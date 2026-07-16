using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using KReca.Data.Entities;
using KReca.Data.Interfaces;

namespace KReca.Business.Services;

public class KategoriService : IKategoriService
{
    private readonly IKategoriRepository _kategoriRepository;

    public KategoriService(IKategoriRepository kategoriRepository)
    {
        _kategoriRepository = kategoriRepository;
    }

    public async Task<List<KategoriDto>> HepsiniGetirAsync()
    {
        var kategoriler = await _kategoriRepository.HepsiniGetirAsync();
        return kategoriler.Select(k => new KategoriDto
        {
            Id = k.Id,
            Ad = k.Ad,
            Aciklama = k.Aciklama
        }).ToList();
    }

    public async Task<KategoriDto?> IdIleGetirAsync(int id)
    {
        var kategori = await _kategoriRepository.IdIleGetirAsync(id);
        if (kategori == null) return null;

        return new KategoriDto
        {
            Id = kategori.Id,
            Ad = kategori.Ad,
            Aciklama = kategori.Aciklama
        };
    }

    public async Task<KategoriDto> EkleAsync(KategoriOlusturDto dto)
    {
        var kategori = new Kategori
        {
            Ad = dto.Ad,
            Aciklama = dto.Aciklama
        };

        var eklenen = await _kategoriRepository.EkleAsync(kategori);
        return new KategoriDto
        {
            Id = eklenen.Id,
            Ad = eklenen.Ad,
            Aciklama = eklenen.Aciklama
        };
    }

    public async Task<bool> SilAsync(int id)
    {
        var kategori = await _kategoriRepository.IdIleGetirAsync(id);
        if (kategori == null) return false;

        await _kategoriRepository.SilAsync(id);
        return true;
    }
}