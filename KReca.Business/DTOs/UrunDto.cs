namespace KReca.Business.DTOs;

public class UrunDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public decimal Fiyat { get; set; }
    public int StokAdedi { get; set; }
    public string? ResimUrl { get; set; }
    public bool AktifMi { get; set; }
    public int KategoriId { get; set; }
    public string KategoriAd { get; set; } = string.Empty;
}

public class UrunOlusturDto
{
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public decimal Fiyat { get; set; }
    public int StokAdedi { get; set; }
    public string? ResimUrl { get; set; }
    public int KategoriId { get; set; }
}

public class UrunGuncelleDto
{
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
    public decimal Fiyat { get; set; }
    public int StokAdedi { get; set; }
    public string? ResimUrl { get; set; }
    public bool AktifMi { get; set; }
    public int KategoriId { get; set; }
}