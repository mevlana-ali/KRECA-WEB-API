using System.Security.Cryptography;
using System.Text;
using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using KReca.Data.Entities;
using KReca.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KReca.Business.Services;

public class PayTRService : IPayTRService
{
    private readonly IConfiguration _config;
    private readonly ISiparisService _siparisService;
    private readonly ISiparisRepository _siparisRepository;
    private readonly ILogger<PayTRService> _logger;

    private string MerchantId => _config["PayTR:MerchantId"]!;
    private string MerchantKey => _config["PayTR:MerchantKey"]!;
    private string MerchantSalt => _config["PayTR:MerchantSalt"]!;
    private string TestMode => _config["PayTR:TestMode"]!;
    private string BaseUrl => _config["PayTR:BaseUrl"]!;

    public PayTRService(
        IConfiguration config,
        ISiparisRepository siparisRepository,
        ISiparisService siparisService,
        ILogger<PayTRService> logger)
    {
        _config = config;
        _siparisRepository = siparisRepository;
        _siparisService = siparisService;
        _logger = logger;
    }

    public async Task<PayTRTokenDto> TokenAlAsync(PayTROdemeBaslatDto dto)
    {
        var siparis = await _siparisRepository.DetaylarIleGetirAsync(dto.SiparisId);
        if (siparis == null)
            return new PayTRTokenDto { Basarili = false, Hata = "Sipariş bulunamadı" };

        var sepetUrunleri = siparis.SiparisDetaylar.Select(sd => new object[]
        {
        sd.Urun?.Ad ?? "Ürün",
        sd.BirimFiyat.ToString("F2"),
        sd.Adet.ToString()
        }).ToList();

        var sepetJson = System.Text.Json.JsonSerializer.Serialize(sepetUrunleri);
        var sepetBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(sepetJson));
        var tutarKurus = ((int)(siparis.GenelToplam * 100)).ToString();

        var hashStr = MerchantId +
                      dto.KullaniciIp +
                      siparis.PayTRMerchantOid +
                      siparis.Email +
                      tutarKurus +
                      sepetBase64 +
                      "0" +
                      "0" +
                      "TL" +
                      TestMode +
                      MerchantSalt;

        var hash = Convert.ToBase64String(
            new HMACSHA256(Encoding.UTF8.GetBytes(MerchantKey))
                .ComputeHash(Encoding.UTF8.GetBytes(hashStr)));

        var formData = new Dictionary<string, string>
    {
        { "merchant_id", MerchantId },
        { "user_ip", dto.KullaniciIp },
        { "merchant_oid", siparis.PayTRMerchantOid! },
        { "email", siparis.Email },
        { "payment_amount", tutarKurus },
        { "paytr_token", hash },
        { "user_basket", sepetBase64 },
        { "debug_on", "1" },
        { "no_installment", "0" },
        { "max_installment", "0" },
        { "user_name", siparis.AdSoyad },
        { "user_address", siparis.Adres },
        { "user_phone", siparis.Telefon },
        { "merchant_ok_url", _config["PayTR:OkUrl"]! },
        { "merchant_fail_url", _config["PayTR:FailUrl"]! },
        { "merchant_notify_url", _config["PayTR:CallbackUrl"]! },
        { "timeout_limit", "30" },
        { "currency", "TL" },
        { "test_mode", TestMode }
    };

        using var client = new HttpClient();
        var response = await client.PostAsync(BaseUrl, new FormUrlEncodedContent(formData));
        var responseStr = await response.Content.ReadAsStringAsync();

        _logger.LogInformation("PayTR Token Response: {Response}", responseStr);

        var result = System.Text.Json.JsonSerializer
            .Deserialize<Dictionary<string, string>>(responseStr);

        if (result?["status"] == "success")
            return new PayTRTokenDto { Basarili = true, Token = result["token"] };

        _logger.LogWarning("PayTR Token Hatası: {Reason}", result?["reason"] ?? "Bilinmeyen");

        return new PayTRTokenDto
        {
            Basarili = false,
            Hata = result?["reason"] ?? "Bilinmeyen hata"
        };
    }

    public async Task<bool> CallbackDogrulaAsync(PayTRCallbackDto callback)
    {
        _logger.LogInformation(
            "PayTR Callback alındı — MerchantOid: {Oid}, Status: {Status}, Amount: {Amount}",
            callback.merchant_oid, callback.status, callback.total_amount);

        // Hash doğrula
        var hashStr = callback.merchant_oid +
                      MerchantSalt +
                      callback.status +
                      callback.total_amount;

        var beklenenHash = Convert.ToBase64String(
            new HMACSHA256(Encoding.UTF8.GetBytes(MerchantKey))
                .ComputeHash(Encoding.UTF8.GetBytes(hashStr)));

        if (beklenenHash != callback.hash)
        {
            _logger.LogWarning(
                "PayTR Hash doğrulama başarısız — MerchantOid: {Oid}", callback.merchant_oid);
            return false;
        }

        // ✅ Artık SiparisService üzerinden düzgün güncelleme yapılıyor
        var basarili = callback.status == "success";
        var sonuc = await _siparisService.OdemeGuncelleAsync(callback.merchant_oid, basarili);

        if (!sonuc)
        {
            _logger.LogError(
                "PayTR Callback — Sipariş bulunamadı: {Oid}", callback.merchant_oid);
        }
        else
        {
            _logger.LogInformation(
                "PayTR Callback — Sipariş güncellendi: {Oid}, Başarılı: {Basarili}",
                callback.merchant_oid, basarili);
        }

        return true;
    }
}