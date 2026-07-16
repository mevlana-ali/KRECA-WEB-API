using KReca.Data.Entities;

namespace KReca.Data.Interfaces;

public interface IUrunRepository : IGenericRepository<Urun>
{
    Task<List<Urun>> KategoriIleGetirAsync(int kategoriId);
    Task<List<Urun>> AktifUrunleriGetirAsync();
}