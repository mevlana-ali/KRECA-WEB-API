using KReca.Data.Entities;
using KReca.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KReca.Data.Repositories;

public class UrunRepository : GenericRepository<Urun>, IUrunRepository
{
    public UrunRepository(AppDbContext context) : base(context) { }

    public async Task<List<Urun>> KategoriIleGetirAsync(int kategoriId)
        => await _context.Urunler
            .Where(u => u.KategoriId == kategoriId && u.AktifMi)
            .Include(u => u.Kategori)
            .ToListAsync();

    public async Task<List<Urun>> AktifUrunleriGetirAsync()
        => await _context.Urunler
            .Where(u => u.AktifMi)
            .Include(u => u.Kategori)
            .ToListAsync();
}