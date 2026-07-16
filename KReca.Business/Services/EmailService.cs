using KReca.Business.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace KReca.Business.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task GonderAsync(string alici, string konu, string icerik)
    {
        var mesaj = new MimeMessage();
        mesaj.From.Add(new MailboxAddress(
            _config["Email:GonderenAd"] ?? "K-RECA",
            _config["Email:Username"] ?? throw new InvalidOperationException("Email:Username yapılandırılmamış")
        ));
        mesaj.To.Add(MailboxAddress.Parse(alici));
        mesaj.Subject = konu;

        mesaj.Body = new TextPart("html") { Text = icerik };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(
            _config["Email:Host"] ?? throw new InvalidOperationException("Email:Host yapılandırılmamış"),
            int.Parse(_config["Email:Port"] ?? "587"),
            SecureSocketOptions.StartTls
        );
        await smtp.AuthenticateAsync(
            _config["Email:Username"] ?? "",
            _config["Email:Password"] ?? ""
        );
        await smtp.SendAsync(mesaj);
        await smtp.DisconnectAsync(true);
    }

    private string UrunTablosuOlustur(List<(string UrunAd, int Adet, decimal BirimFiyat)> urunler)
    {
        var satirlar = string.Join("", urunler.Select(u =>
            $@"<tr>
                <td style='padding: 10px 12px; border-bottom: 1px solid #eee; color: #333;'>{u.UrunAd}</td>
                <td style='padding: 10px 12px; border-bottom: 1px solid #eee; color: #555; text-align: center;'>{u.Adet}</td>
                <td style='padding: 10px 12px; border-bottom: 1px solid #eee; color: #333; text-align: right;'>₺{(u.BirimFiyat * u.Adet):F2}</td>
            </tr>"));

        return $@"
            <table style='width: 100%; border-collapse: collapse; margin: 15px 0;'>
                <thead>
                    <tr style='background: #f0f0f0;'>
                        <th style='padding: 10px 12px; text-align: left; color: #1a2b4a; font-size: 13px;'>Ürün</th>
                        <th style='padding: 10px 12px; text-align: center; color: #1a2b4a; font-size: 13px;'>Adet</th>
                        <th style='padding: 10px 12px; text-align: right; color: #1a2b4a; font-size: 13px;'>Tutar</th>
                    </tr>
                </thead>
                <tbody>{satirlar}</tbody>
            </table>";
    }

    public async Task SiparisOnayiGonderAsync(
        string alici, string adSoyad, string adres, string sehir, string? faturaAdresi, string? faturaSehri, string? firmaAdi, string? tcVergiNo,
        List<(string UrunAd, int Adet, decimal BirimFiyat)> urunler,
        decimal araToplam, decimal kargoUcreti, decimal genelToplam)
    {
        var urunTablosu = UrunTablosuOlustur(urunler);
        var kargoText = kargoUcreti == 0
            ? "<span style='color: #2ecc71; font-weight: bold;'>Ücretsiz</span>"
            : $"₺{kargoUcreti:F2}";

        var konu = "Siparişiniz Alındı — K-RECA Tıbbi Sülük";
        var icerik = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background: #1a2b4a; padding: 30px; text-align: center;'>
                <h1 style='color: #2ecc71; margin: 0;'>K-RECA Tıbbi Sülük</h1>
            </div>
            <div style='padding: 30px; background: #f9f9f9;'>
                <h2 style='color: #1a2b4a; margin-top: 0;'>Sayın {adSoyad},</h2>
                <p style='color: #555;'>Siparişiniz başarıyla alınmıştır. En kısa sürede hazırlanacaktır.</p>

                <div style='background: #fff; border-radius: 8px; padding: 20px; margin: 20px 0; border: 1px solid #eee;'>
                    <h3 style='color: #1a2b4a; margin: 0 0 10px; font-size: 15px;'>📦 Sipariş Detayları</h3>
                    {urunTablosu}
                    <div style='border-top: 2px solid #eee; padding-top: 12px; margin-top: 8px;'>
                        <div style='display: flex; justify-content: space-between; margin-bottom: 6px;'>
                            <span style='color: #777; font-size: 13px;'>Ara Toplam:</span>
                            <span style='color: #333; font-size: 13px;'>₺{araToplam:F2}</span>
                        </div>
                        <div style='display: flex; justify-content: space-between; margin-bottom: 6px;'>
                            <span style='color: #777; font-size: 13px;'>Kargo:</span>
                            <span style='font-size: 13px;'>{kargoText}</span>
                        </div>
                        <div style='display: flex; justify-content: space-between; border-top: 1px solid #eee; padding-top: 8px; margin-top: 4px;'>
                            <span style='color: #1a2b4a; font-weight: bold;'>Genel Toplam:</span>
                            <span style='color: #1a2b4a; font-weight: bold; font-size: 16px;'>₺{genelToplam:F2}</span>
                        </div>
                    </div>
                </div>

                <div style='background: #fff; border-radius: 8px; padding: 15px; margin: 15px 0; border: 1px solid #eee; display: flex; flex-wrap: wrap; gap: 20px;'>
                    <div style='flex: 1; min-width: 250px;'>
                        <h3 style='color: #1a2b4a; margin: 0 0 8px; font-size: 14px;'>📍 Teslimat Adresi</h3>
                        <p style='margin: 0; color: #555; font-size: 13px;'>{adres}, {sehir}</p>
                    </div>
                    <div style='flex: 1; min-width: 250px;'>
                        <h3 style='color: #1a2b4a; margin: 0 0 8px; font-size: 14px;'>🧾 Fatura Adresi</h3>
                        {(firmaAdi != null ? $"<p style='margin: 0 0 4px; color: #555; font-size: 13px;'><strong>Firma:</strong> {firmaAdi}</p>" : "")}
                        {(tcVergiNo != null ? $"<p style='margin: 0 0 4px; color: #555; font-size: 13px;'><strong>TC/Vergi No:</strong> {tcVergiNo}</p>" : "")}
                        <p style='margin: 0; color: #555; font-size: 13px;'>{faturaAdresi ?? adres}, {faturaSehri ?? sehir}</p>
                    </div>
                </div>

                <p style='color: #555; font-size: 13px;'>Siparişinizin durumu değiştiğinde e-posta ile bilgilendirileceksiniz.</p>
            </div>
            <div style='background: #1a2b4a; padding: 20px; text-align: center;'>
                <p style='color: #aaa; margin: 0; font-size: 12px;'>K-RECA Tıbbi Sülük | krecasuluk54@gmail.com | +90 541 614 87 91</p>
            </div>
        </div>";

        await GonderAsync(alici, konu, icerik);
    }

    public async Task SiparisAdminBildirimiGonderAsync(
        string adminEmail, string adSoyad, string telefon, string sehir,
        List<(string UrunAd, int Adet, decimal BirimFiyat)> urunler,
        decimal genelToplam)
    {
        var urunTablosu = UrunTablosuOlustur(urunler);

        var konu = $"YENİ SİPARİŞ — K-RECA ({genelToplam:F2} ₺)";
        var icerik = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background: #e67e22; padding: 20px; text-align: center;'>
                <h1 style='color: white; margin: 0;'>🎉 Yeni Bir Sipariş Geldi!</h1>
            </div>
            <div style='padding: 30px; background: #f9f9f9;'>
                <p style='color: #555; font-size: 16px;'>Web sitenizden başarılı bir ödeme gerçekleşti ve yeni bir sipariş oluşturuldu.</p>
                
                <div style='background: #fff; border-radius: 8px; padding: 20px; margin: 20px 0; border-left: 4px solid #e67e22;'>
                    <h3 style='color: #1a2b4a; margin: 0 0 10px; font-size: 15px;'>Müşteri Bilgileri</h3>
                    <p style='margin: 0 0 5px; color: #333;'><strong>Ad Soyad:</strong> {adSoyad}</p>
                    <p style='margin: 0 0 5px; color: #333;'><strong>Telefon:</strong> <a href='tel:{telefon}'>{telefon}</a></p>
                    <p style='margin: 0 0 5px; color: #333;'><strong>Şehir:</strong> {sehir}</p>
                </div>

                <div style='background: #fff; border-radius: 8px; padding: 20px; margin: 15px 0; border: 1px solid #eee;'>
                    <h3 style='color: #1a2b4a; margin: 0 0 10px; font-size: 15px;'>📦 Sipariş İçeriği</h3>
                    {urunTablosu}
                    <div style='border-top: 2px solid #eee; padding-top: 10px; text-align: right;'>
                        <span style='color: #e67e22; font-weight: bold; font-size: 18px;'>Toplam Tutar: ₺{genelToplam:F2}</span>
                    </div>
                </div>

                <p style='color: #555; font-size: 13px;'>Siparişin tüm detaylarını görüntülemek ve yönetmek için admin paneline giriş yapabilirsiniz.</p>
                <div style='text-align: center; margin-top: 20px;'>
                    <a href='https://k-reca.com/yonetim-girisi-kreca' style='display: inline-block; background: #1a2b4a; color: white; padding: 12px 24px; border-radius: 8px; text-decoration: none; font-weight: bold;'>Admin Paneline Git</a>
                </div>
            </div>
        </div>";

        await GonderAsync(adminEmail, konu, icerik);
    }

    public async Task DurumGuncelleGonderAsync(
        string alici, string adSoyad, string durum,
        List<(string UrunAd, int Adet, decimal BirimFiyat)> urunler,
        decimal genelToplam)
    {
        var (baslik, aciklama, renk) = durum switch
        {
            "Onaylandi" => ("Siparişiniz Onaylandı ✓", "Ödemeniz alındı, siparişiniz hazırlanıyor.", "#2ecc71"),
            "Kargoda" => ("Siparişiniz Kargoya Verildi 🚚", "Siparişiniz kargoya verilmiştir. Yakında teslim edilecektir.", "#e67e22"),
            "TeslimEdildi" => ("Siparişiniz Teslim Edildi ✅", "Siparişiniz teslim edilmiştir. Teşekkür ederiz!", "#27ae60"),
            "Iptal" => ("Siparişiniz İptal Edildi", "Siparişiniz iptal edilmiştir. Detay için bize ulaşabilirsiniz.", "#e74c3c"),
            _ => ("Sipariş Durumu Güncellendi", "Siparişinizin durumu güncellenmiştir.", "#1a2b4a")
        };

        var urunTablosu = UrunTablosuOlustur(urunler);

        var konu = $"{baslik} — K-RECA";
        var icerik = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <div style='background: #1a2b4a; padding: 30px; text-align: center;'>
                <h1 style='color: #2ecc71; margin: 0;'>K-RECA Tıbbi Sülük</h1>
            </div>
            <div style='padding: 30px; background: #f9f9f9;'>
                <h2 style='color: #1a2b4a; margin-top: 0;'>Sayın {adSoyad},</h2>
                <div style='background: #fff; border-radius: 8px; padding: 20px; margin: 20px 0; border-left: 4px solid {renk};'>
                    <h3 style='color: {renk}; margin: 0 0 10px;'>{baslik}</h3>
                    <p style='margin: 0; color: #555;'>{aciklama}</p>
                </div>

                <div style='background: #fff; border-radius: 8px; padding: 20px; margin: 15px 0; border: 1px solid #eee;'>
                    <h3 style='color: #1a2b4a; margin: 0 0 10px; font-size: 15px;'>📦 Sipariş Detayları</h3>
                    {urunTablosu}
                    <div style='border-top: 2px solid #eee; padding-top: 10px; text-align: right;'>
                        <span style='color: #1a2b4a; font-weight: bold; font-size: 15px;'>Toplam: ₺{genelToplam:F2}</span>
                    </div>
                </div>

                <p style='color: #555; font-size: 13px;'>Sorularınız için bize WhatsApp veya telefon ile ulaşabilirsiniz.</p>
                <a href='https://wa.me/905416148791' 
                   style='display: inline-block; background: #25d366; color: white; padding: 12px 24px; border-radius: 8px; text-decoration: none; font-weight: bold;'>
                    WhatsApp ile Yazın
                </a>
            </div>
            <div style='background: #1a2b4a; padding: 20px; text-align: center;'>
                <p style='color: #aaa; margin: 0; font-size: 12px;'>K-RECA Tıbbi Sülük | krecasuluk54@gmail.com | +90 541 614 87 91</p>
            </div>
        </div>";

        await GonderAsync(alici, konu, icerik);
    }
}