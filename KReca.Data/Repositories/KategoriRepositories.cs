using KReca.Data.Entities;
using KReca.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KReca.Data.Repositories;

public class KategoriRepository : GenericRepository<Kategori>, IKategoriRepository
{
    public KategoriRepository(AppDbContext context) : base(context) { }

    public async Task<Kategori?> UrunlerIleGetirAsync(int id)
        => await _context.Kategoriler
            .Include(k => k.Urunler)
            .FirstOrDefaultAsync(k => k.Id == id);
}