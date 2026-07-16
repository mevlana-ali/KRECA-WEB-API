using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using KReca.Data.Interfaces;

namespace KReca.Business.Services;

public class SiteAyarlariService : ISiteAyarlariService
{
    private readonly ISiteAyarlariRepository _repo;

    public SiteAyarlariService(ISiteAyarlariRepository repo)
    {
        _repo = repo;
    }

    public async Task<SiteAyarlariDto> GetirAsync()
    {
        var ayarlar = await _repo.GetirAsync();
        return new SiteAyarlariDto
        {
            KargoUcreti = ayarlar.KargoUcreti,
            UcretsizKargoLimiti = ayarlar.UcretsizKargoLimiti,
            UcretsizKargoAktif = ayarlar.UcretsizKargoAktif,
            DuyuruMetni = ayarlar.DuyuruMetni,
            GuncellenmeTarihi = ayarlar.GuncellenmeTarihi
        };
    }

    public async Task<SiteAyarlariDto> GuncelleAsync(SiteAyarlariGuncelleDto dto)
    {
        var ayarlar = await _repo.GetirAsync();
        ayarlar.KargoUcreti = dto.KargoUcreti;
        ayarlar.UcretsizKargoLimiti = dto.UcretsizKargoLimiti;
        ayarlar.UcretsizKargoAktif = dto.UcretsizKargoAktif;
        ayarlar.DuyuruMetni = dto.DuyuruMetni;

        await _repo.GuncelleAsync(ayarlar);

        return new SiteAyarlariDto
        {
            KargoUcreti = ayarlar.KargoUcreti,
            UcretsizKargoLimiti = ayarlar.UcretsizKargoLimiti,
            UcretsizKargoAktif = ayarlar.UcretsizKargoAktif,
            DuyuruMetni = ayarlar.DuyuruMetni,
            GuncellenmeTarihi = ayarlar.GuncellenmeTarihi
        };
    }

    public async Task<KargoAyarlariDto> KargoAyarlariGetirAsync()
    {
        var ayarlar = await _repo.GetirAsync();
        return new KargoAyarlariDto
        {
            KargoUcreti = ayarlar.KargoUcreti,
            UcretsizKargoLimiti = ayarlar.UcretsizKargoLimiti,
            UcretsizKargoAktif = ayarlar.UcretsizKargoAktif
        };
    }

    public async Task<decimal> KargoUcretiHesaplaAsync(decimal siparisToplami)
    {
        var ayarlar = await _repo.GetirAsync();

        if (ayarlar.UcretsizKargoAktif && siparisToplami >= ayarlar.UcretsizKargoLimiti)
            return 0;

        return ayarlar.KargoUcreti;
    }
}
