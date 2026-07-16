using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface ISiparisService
{
    Task<List<SiparisDto>> HepsiniGetirAsync();
    Task<SiparisDto?> IdIleGetirAsync(int id);
    Task<SiparisDto> OlusturAsync(SiparisOlusturDto dto);
    Task<bool> OdemeGuncelleAsync(string merchantOid, bool basarili);
    Task<bool> DurumGuncelleAsync(int id, SiparisDurumGuncelleDto dto);
}