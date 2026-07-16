using KReca.Data.Entities;
using KReca.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KReca.Data.Repositories;

public class SiteAyarlariRepository : ISiteAyarlariRepository
{
    private readonly AppDbContext _context;

    public SiteAyarlariRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SiteAyarlari> GetirAsync()
    {
        var ayarlar = await _context.SiteAyarlari.FirstOrDefaultAsync();
        if (ayarlar == null)
        {
            // Yoksa varsayılan oluştur
            ayarlar = new SiteAyarlari
            {
                KargoUcreti = 150,
                UcretsizKargoLimiti = 1000,
                UcretsizKargoAktif = false
            };
            await _context.SiteAyarlari.AddAsync(ayarlar);
            await _context.SaveChangesAsync();
        }
        return ayarlar;
    }

    public async Task<SiteAyarlari> GuncelleAsync(SiteAyarlari ayarlar)
    {
        ayarlar.GuncellenmeTarihi = DateTime.UtcNow;
        _context.SiteAyarlari.Update(ayarlar);
        await _context.SaveChangesAsync();
        return ayarlar;
    }
}
