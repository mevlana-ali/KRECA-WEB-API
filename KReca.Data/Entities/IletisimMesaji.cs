namespace KReca.Data.Entities;

public class IletisimMesaji
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Mesaj { get; set; } = string.Empty;
    public bool Okundu { get; set; } = false;
    public DateTime OlusturulmaTarihi { get; set; } = DateTime.UtcNow;
}
