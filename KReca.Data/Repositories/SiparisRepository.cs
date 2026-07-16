using KReca.Data.Entities;
using KReca.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KReca.Data.Repositories;

public class SiparisRepository : GenericRepository<Siparis>, ISiparisRepository
{
    public SiparisRepository(AppDbContext context) : base(context) { }

    public async Task<List<Siparis>> TumunuDetaylarIleGetirAsync()
        => await _context.Siparisler
            .Include(s => s.SiparisDetaylar)
            .ThenInclude(sd => sd.Urun)
            .OrderByDescending(s => s.OlusturulmaTarihi)
            .ToListAsync();

    public async Task<Siparis?> DetaylarIleGetirAsync(int id)
        => await _context.Siparisler
            .Include(s => s.SiparisDetaylar)
            .ThenInclude(sd => sd.Urun)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Siparis?> MerchantOidIleGetirAsync(string merchantOid)
        => await _context.Siparisler
            .Include(s => s.SiparisDetaylar)
            .ThenInclude(sd => sd.Urun)
            .FirstOrDefaultAsync(s => s.PayTRMerchantOid == merchantOid);
}