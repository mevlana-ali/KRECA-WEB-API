using KReca.Data.Entities;

namespace KReca.Data.Interfaces;

public interface IIletisimMesajiRepository : IGenericRepository<IletisimMesaji>
{
    Task<List<IletisimMesaji>> OkunmamislariGetirAsync();
}
