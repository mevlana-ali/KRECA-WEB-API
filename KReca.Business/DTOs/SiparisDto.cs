using System.ComponentModel.DataAnnotations;

namespace KReca.Business.DTOs;

public class SiparisOlusturDto
{
    [Required(ErrorMessage = "Ad Soyad zorunludur")]
    [MinLength(3, ErrorMessage = "Ad Soyad en az 3 karakter olmalı")]
    [MaxLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
    public string AdSoyad { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon zorunludur")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası girin")]
    [MaxLength(20)]
    public string Telefon { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres zorunludur")]
    [MinLength(10, ErrorMessage = "Adres en az 10 karakter olmalı")]
    [MaxLength(500)]
    public string Adres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şehir zorunludur")]
    [MaxLength(50)]
    public string Sehir { get; set; } = string.Empty;

    [Required(ErrorMessage = "Posta kodu zorunludur")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Posta kodu 5 haneli olmalı")]
    public string PostaKodu { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sepet boş olamaz")]
    [MinLength(1, ErrorMessage = "En az 1 ürün gereklidir")]
    public List<SepetItemDto> SepetItems { get; set; } = new();

    // Fatura Alanları
    public bool FaturaAdresiFarkli { get; set; }
    
    [MaxLength(500)]
    public string? FaturaAdresi { get; set; }
    
    [MaxLength(50)]
    public string? FaturaSehri { get; set; }
    
    [MaxLength(50)]
    public string? TcVeyaVergiNo { get; set; }
    
    [MaxLength(200)]
    public string? FirmaAdi { get; set; }
}

public class SepetItemDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Ürün ID geçersiz")]
    public int UrunId { get; set; }

    [Range(1, 1000, ErrorMessage = "Adet 1 ile 1000 arasında olmalı")]
    public int Adet { get; set; }
}

public class SiparisDto
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Adres { get; set; } = string.Empty;
    public string Sehir { get; set; } = string.Empty;
    public decimal ToplamTutar { get; set; }
    public decimal KargoUcreti { get; set; }
    public decimal GenelToplam { get; set; }
    public string Durum { get; set; } = string.Empty;
    public bool OdemeTamamlandi { get; set; }
    public DateTime OlusturulmaTarihi { get; set; }

    // Fatura Alanları
    public string? FaturaAdresi { get; set; }
    public string? FaturaSehri { get; set; }
    public string? TcVeyaVergiNo { get; set; }
    public string? FirmaAdi { get; set; }

    public List<SiparisDetayDto> Detaylar { get; set; } = new();
}

public class SiparisDetayDto
{
    public int UrunId { get; set; }
    public string UrunAd { get; set; } = string.Empty;
    public int Adet { get; set; }
    public decimal BirimFiyat { get; set; }
    public decimal ToplamFiyat { get; set; }
}

public class SiparisDurumGuncelleDto
{
    [Range(0, 6, ErrorMessage = "Geçersiz durum değeri")]
    public int Durum { get; set; }
}