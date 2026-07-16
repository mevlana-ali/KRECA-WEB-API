namespace KReca.Data.Entities;

public class SiteAyarlari
{
    public int Id { get; set; }

    // Kargo Ayarları
    public decimal KargoUcreti { get; set; } = 150;
    public decimal UcretsizKargoLimiti { get; set; } = 0; // 0 = devre dışı
    public bool UcretsizKargoAktif { get; set; } = false;

    // İletişim Mesajları
    public string? DuyuruMetni { get; set; } // Üst banner için

    // Genel
    public DateTime GuncellenmeTarihi { get; set; } = DateTime.UtcNow;
}
