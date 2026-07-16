using System.ComponentModel.DataAnnotations;

namespace KReca.Business.DTOs;

public class SiteAyarlariDto
{
    public decimal KargoUcreti { get; set; }
    public decimal UcretsizKargoLimiti { get; set; }
    public bool UcretsizKargoAktif { get; set; }
    public string? DuyuruMetni { get; set; }
    public DateTime GuncellenmeTarihi { get; set; }
}

public class SiteAyarlariGuncelleDto
{
    [Range(0, 10000, ErrorMessage = "Kargo ücreti 0-10000 arasında olmalı")]
    public decimal KargoUcreti { get; set; }

    [Range(0, 100000, ErrorMessage = "Ücretsiz kargo limiti 0-100000 arasında olmalı")]
    public decimal UcretsizKargoLimiti { get; set; }

    public bool UcretsizKargoAktif { get; set; }

    [MaxLength(500, ErrorMessage = "Duyuru metni en fazla 500 karakter olabilir")]
    public string? DuyuruMetni { get; set; }
}

// Kargo bilgisi — frontend için public endpoint
public class KargoAyarlariDto
{
    public decimal KargoUcreti { get; set; }
    public decimal UcretsizKargoLimiti { get; set; }
    public bool UcretsizKargoAktif { get; set; }
}
