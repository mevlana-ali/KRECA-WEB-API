using KReca.Data.Entities;
using KReca.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KReca.Data.Repositories;

public class IletisimMesajiRepository : GenericRepository<IletisimMesaji>, IIletisimMesajiRepository
{
    public IletisimMesajiRepository(AppDbContext context) : base(context) { }

    public async Task<List<IletisimMesaji>> OkunmamislariGetirAsync()
    {
        return await _dbSet
            .Where(m => !m.Okundu)
            .OrderByDescending(m => m.OlusturulmaTarihi)
            .ToListAsync();
    }
}
