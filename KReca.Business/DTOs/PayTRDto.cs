namespace KReca.Business.DTOs;

public class PayTROdemeBaslatDto
{
    public int SiparisId { get; set; }
    public string KullaniciIp { get; set; } = string.Empty;
}

public class PayTRTokenDto
{
    public bool Basarili { get; set; }
    public string? Token { get; set; }
    public string? Hata { get; set; }
}

public class PayTRCallbackDto
{
    public string merchant_oid { get; set; } = string.Empty;
    public string status { get; set; } = string.Empty;
    public string total_amount { get; set; } = string.Empty;
    public string hash { get; set; } = string.Empty;
}