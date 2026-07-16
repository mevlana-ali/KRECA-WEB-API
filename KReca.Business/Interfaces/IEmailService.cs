namespace KReca.Business.Interfaces;

public interface IEmailService
{
    Task GonderAsync(string alici, string konu, string icerik);
    Task SiparisOnayiGonderAsync(string alici, string adSoyad, string adres, string sehir, string? faturaAdresi, string? faturaSehri, string? firmaAdi, string? tcVergiNo, List<(string UrunAd, int Adet, decimal BirimFiyat)> urunler, decimal araToplam, decimal kargoUcreti, decimal genelToplam);
    Task SiparisAdminBildirimiGonderAsync(string adminEmail, string adSoyad, string telefon, string sehir, List<(string UrunAd, int Adet, decimal BirimFiyat)> urunler, decimal genelToplam);
    Task DurumGuncelleGonderAsync(string alici, string adSoyad, string durum, List<(string UrunAd, int Adet, decimal BirimFiyat)> urunler, decimal genelToplam);
}