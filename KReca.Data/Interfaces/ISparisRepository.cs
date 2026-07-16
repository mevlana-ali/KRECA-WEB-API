using KReca.Data.Entities;

namespace KReca.Data.Interfaces;

public interface ISiparisRepository : IGenericRepository<Siparis>
{
    Task<List<Siparis>> TumunuDetaylarIleGetirAsync();
    Task<Siparis?> DetaylarIleGetirAsync(int id);
    Task<Siparis?> MerchantOidIleGetirAsync(string merchantOid);
}