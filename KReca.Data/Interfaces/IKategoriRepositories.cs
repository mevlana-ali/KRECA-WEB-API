using KReca.Data.Entities;

namespace KReca.Data.Interfaces;

public interface IKategoriRepository : IGenericRepository<Kategori>
{
    Task<Kategori?> UrunlerIleGetirAsync(int id);
}