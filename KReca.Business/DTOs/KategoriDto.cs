namespace KReca.Business.DTOs;

public class KategoriDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
}

public class KategoriOlusturDto
{
    public string Ad { get; set; } = string.Empty;
    public string? Aciklama { get; set; }
}