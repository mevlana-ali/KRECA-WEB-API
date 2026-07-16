using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface ISiteAyarlariService
{
    Task<SiteAyarlariDto> GetirAsync();
    Task<SiteAyarlariDto> GuncelleAsync(SiteAyarlariGuncelleDto dto);
    Task<KargoAyarlariDto> KargoAyarlariGetirAsync();
    Task<decimal> KargoUcretiHesaplaAsync(decimal siparisToplami);
}
