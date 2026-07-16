using KReca.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KReca.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> HepsiniGetirAsync()
        => await _dbSet.ToListAsync();

    public async Task<T?> IdIleGetirAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task<T> EkleAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> GuncelleAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task SilAsync(int id)
    {
        var entity = await IdIleGetirAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}