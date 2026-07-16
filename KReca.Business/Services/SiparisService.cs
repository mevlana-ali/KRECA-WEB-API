using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using KReca.Data.Entities;
using KReca.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace KReca.Business.Services;

public class SiparisService : ISiparisService
{
    private readonly ISiparisRepository _siparisRepository;
    private readonly IUrunRepository _urunRepository;
    private readonly IEmailService _emailService;
    private readonly ISiteAyarlariService _siteAyarlariService;
    private readonly ILogger<SiparisService> _logger;

    public SiparisService(
        ISiparisRepository siparisRepository,
        IUrunRepository urunRepository,
        IEmailService emailService,
        ISiteAyarlariService siteAyarlariService,
        ILogger<SiparisService> logger)
    {
        _siparisRepository = siparisRepository;
        _urunRepository = urunRepository;
        _emailService = emailService;
        _siteAyarlariService = siteAyarlariService;
        _logger = logger;
    }

    public async Task<List<SiparisDto>> HepsiniGetirAsync()
    {
        var siparisler = await _siparisRepository.TumunuDetaylarIleGetirAsync();
        return siparisler.Select(SiparisedonusturDto).ToList();
    }

    public async Task<SiparisDto?> IdIleGetirAsync(int id)
    {
        var siparis = await _siparisRepository.DetaylarIleGetirAsync(id);
        if (siparis == null) return null;
        return SiparisedonusturDto(siparis);
    }

    public async Task<SiparisDto> OlusturAsync(SiparisOlusturDto dto)
    {
        // Validasyon
        if (dto.SepetItems == null || dto.SepetItems.Count == 0)
            throw new InvalidOperationException("Sepet boş olamaz.");

        // Önce ürün toplamını hesapla, sonra kargo ücretini belirle
        decimal toplam = 0;
        var sepetDetaylar = new List<(int UrunId, int Adet, decimal BirimFiyat, decimal ToplamFiyat)>();

        foreach (var item in dto.SepetItems)
        {
            var urun = await _urunRepository.IdIleGetirAsync(item.UrunId);
            if (urun == null)
                throw new InvalidOperationException($"Ürün bulunamadı: ID {item.UrunId}");

            if (urun.StokAdedi < item.Adet)
                throw new InvalidOperationException(
                    $"'{urun.Ad}' için yeterli stok yok. Mevcut: {urun.StokAdedi}, İstenen: {item.Adet}");

            var itemToplam = urun.Fiyat * item.Adet;
            toplam += itemToplam;
            sepetDetaylar.Add((urun.Id, item.Adet, urun.Fiyat, itemToplam));

            // Stok düşür
            urun.StokAdedi -= item.Adet;
        }

        // Dinamik kargo ücreti hesapla
        var kargoUcreti = await _siteAyarlariService.KargoUcretiHesaplaAsync(toplam);

        var siparis = new Siparis
        {
            AdSoyad = dto.AdSoyad,
            Email = dto.Email,
            Telefon = dto.Telefon,
            Adres = dto.Adres,
            Sehir = dto.Sehir,
            PostaKodu = dto.PostaKodu,
            KargoUcreti = kargoUcreti,
            
            // Fatura Bilgileri
            FaturaAdresi = dto.FaturaAdresiFarkli ? dto.FaturaAdresi : dto.Adres,
            FaturaSehri = dto.FaturaAdresiFarkli ? dto.FaturaSehri : dto.Sehir,
            TcVeyaVergiNo = dto.FaturaAdresiFarkli ? dto.TcVeyaVergiNo : null,
            FirmaAdi = dto.FaturaAdresiFarkli ? dto.FirmaAdi : null,
            
            OdemeTamamlandi = false,
            Durum = SiparisDurumu.Beklemede,
            PayTRMerchantOid = "KR" + DateTime.UtcNow.Ticks
        };

        // Detayları siparişe ekle
        foreach (var sd in sepetDetaylar)
        {
            siparis.SiparisDetaylar.Add(new SiparisDetay
            {
                UrunId = sd.UrunId,
                Adet = sd.Adet,
                BirimFiyat = sd.BirimFiyat,
                ToplamFiyat = sd.ToplamFiyat
            });
        }

        siparis.ToplamTutar = toplam;
        siparis.GenelToplam = toplam + siparis.KargoUcreti;

        var eklenen = await _siparisRepository.EkleAsync(siparis);

        return SiparisedonusturDto(eklenen);
    }

    public async Task<bool> OdemeGuncelleAsync(string merchantOid, bool basarili)
    {
        var siparis = await _siparisRepository.MerchantOidIleGetirAsync(merchantOid);
        if (siparis == null) return false;

        // CRITICAL FIX: Aynı bildirim birden fazla kez gelirse tekrar işlenmesini engelle (Idempotency)
        if (siparis.Durum != SiparisDurumu.Beklemede)
        {
            _logger.LogInformation("Bu siparişin durumu zaten güncellenmiş (Mevcut Durum: {Durum}): {MerchantOid}", siparis.Durum, merchantOid);
            return true; // PayTR'ye OK yanıtı dönmek için true veriyoruz
        }

        siparis.OdemeTamamlandi = basarili;
        siparis.Durum = basarili ? SiparisDurumu.Onaylandi : SiparisDurumu.Iptal;
        await _siparisRepository.GuncelleAsync(siparis);

        // Ödeme başarısızsa stokları geri yükle
        if (!basarili)
        {
            _logger.LogInformation(
                "Ödeme başarısız — stoklar geri yükleniyor: {MerchantOid}", merchantOid);

            foreach (var detay in siparis.SiparisDetaylar)
            {
                if (detay.Urun != null)
                {
                    detay.Urun.StokAdedi += detay.Adet;
                }
            }
            await _siparisRepository.GuncelleAsync(siparis);
        }

        // Müşteriye ve Admin'e e-posta bilgilendirmesi gönder
        try
        {
            var urunListesi = UrunListesiOlustur(siparis);
            
            // Müşteriye gönder
            await _emailService.DurumGuncelleGonderAsync(
                siparis.Email,
                siparis.AdSoyad,
                siparis.Durum.ToString(),
                urunListesi,
                siparis.GenelToplam
            );

            // Eğer ödeme başarılıysa Admin'e bildirim gönder
            if (basarili)
            {
                await _emailService.SiparisAdminBildirimiGonderAsync(
                    "krecasuluk54@gmail.com", // Admin E-posta adresi
                    siparis.AdSoyad,
                    siparis.Telefon,
                    siparis.Sehir,
                    urunListesi,
                    siparis.GenelToplam
                );
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Ödeme durum e-postası gönderilemedi — Sipariş #{SiparisId}", siparis.Id);
        }

        return true;
    }

    public async Task<bool> DurumGuncelleAsync(int id, SiparisDurumGuncelleDto dto)
    {
        var siparis = await _siparisRepository.DetaylarIleGetirAsync(id);
        if (siparis == null) return false;

        siparis.Durum = (SiparisDurumu)dto.Durum;
        await _siparisRepository.GuncelleAsync(siparis);

        // Müşteriye e-posta gönder — hata olursa durumu engellemesin
        try
        {
            var urunListesi = UrunListesiOlustur(siparis);
            await _emailService.DurumGuncelleGonderAsync(
                siparis.Email,
                siparis.AdSoyad,
                siparis.Durum.ToString(),
                urunListesi,
                siparis.GenelToplam
            );
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Durum güncelleme e-postası gönderilemedi — Sipariş #{SiparisId}", siparis.Id);
        }

        return true;
    }

    private List<(string UrunAd, int Adet, decimal BirimFiyat)> UrunListesiOlustur(Siparis siparis)
    {
        return siparis.SiparisDetaylar.Select(sd => (
            UrunAd: sd.Urun?.Ad ?? "Ürün",
            Adet: sd.Adet,
            BirimFiyat: sd.BirimFiyat
        )).ToList();
    }

    private SiparisDto SiparisedonusturDto(Siparis siparis) => new SiparisDto
    {
        Id = siparis.Id,
        AdSoyad = siparis.AdSoyad,
        Email = siparis.Email,
        Telefon = siparis.Telefon,
        Adres = siparis.Adres,
        Sehir = siparis.Sehir,
        ToplamTutar = siparis.ToplamTutar,
        KargoUcreti = siparis.KargoUcreti,
        GenelToplam = siparis.GenelToplam,
        Durum = siparis.Durum.ToString(),
        OdemeTamamlandi = siparis.OdemeTamamlandi,
        OlusturulmaTarihi = siparis.OlusturulmaTarihi,
        FaturaAdresi = siparis.FaturaAdresi,
        FaturaSehri = siparis.FaturaSehri,
        TcVeyaVergiNo = siparis.TcVeyaVergiNo,
        FirmaAdi = siparis.FirmaAdi,
        Detaylar = siparis.SiparisDetaylar.Select(sd => new SiparisDetayDto
        {
            UrunId = sd.UrunId,
            UrunAd = sd.Urun?.Ad ?? string.Empty,
            Adet = sd.Adet,
            BirimFiyat = sd.BirimFiyat,
            ToplamFiyat = sd.ToplamFiyat
        }).ToList()
    };
}