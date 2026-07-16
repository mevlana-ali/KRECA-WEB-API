using KReca.Data.Entities;

namespace KReca.Data.Interfaces;

public interface ISiteAyarlariRepository
{
    Task<SiteAyarlari> GetirAsync();
    Task<SiteAyarlari> GuncelleAsync(SiteAyarlari ayarlar);
}
