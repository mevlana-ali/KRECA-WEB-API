using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using KReca.Data.Entities;
using KReca.Data.Interfaces;

namespace KReca.Business.Services;

public class UrunService : IUrunService
{
    private readonly IUrunRepository _urunRepository;

    public UrunService(IUrunRepository urunRepository)
    {
        _urunRepository = urunRepository;
    }

    public async Task<List<UrunDto>> HepsiniGetirAsync()
    {
        var urunler = await _urunRepository.AktifUrunleriGetirAsync();
        return urunler.Select(u => new UrunDto
        {
            Id = u.Id,
            Ad = u.Ad,
            Aciklama = u.Aciklama,
            Fiyat = u.Fiyat,
            StokAdedi = u.StokAdedi,
            ResimUrl = u.ResimUrl,
            AktifMi = u.AktifMi,
            KategoriId = u.KategoriId,
            KategoriAd = u.Kategori?.Ad ?? string.Empty
        }).ToList();
    }

    public async Task<UrunDto?> IdIleGetirAsync(int id)
    {
        var urun = await _urunRepository.IdIleGetirAsync(id);
        if (urun == null) return null;

        return new UrunDto
        {
            Id = urun.Id,
            Ad = urun.Ad,
            Aciklama = urun.Aciklama,
            Fiyat = urun.Fiyat,
            StokAdedi = urun.StokAdedi,
            ResimUrl = urun.ResimUrl,
            AktifMi = urun.AktifMi,
            KategoriId = urun.KategoriId,
            KategoriAd = urun.Kategori?.Ad ?? string.Empty
        };
    }

    public async Task<List<UrunDto>> KategoriIleGetirAsync(int kategoriId)
    {
        var urunler = await _urunRepository.KategoriIleGetirAsync(kategoriId);
        return urunler.Select(u => new UrunDto
        {
            Id = u.Id,
            Ad = u.Ad,
            Aciklama = u.Aciklama,
            Fiyat = u.Fiyat,
            StokAdedi = u.StokAdedi,
            ResimUrl = u.ResimUrl,
            AktifMi = u.AktifMi,
            KategoriId = u.KategoriId,
            KategoriAd = u.Kategori?.Ad ?? string.Empty
        }).ToList();
    }

    public async Task<UrunDto> EkleAsync(UrunOlusturDto dto)
    {
        var urun = new Urun
        {
            Ad = dto.Ad,
            Aciklama = dto.Aciklama,
            Fiyat = dto.Fiyat,
            StokAdedi = dto.StokAdedi,
            ResimUrl = dto.ResimUrl,
            KategoriId = dto.KategoriId
        };

        var eklenen = await _urunRepository.EkleAsync(urun);
        return await IdIleGetirAsync(eklenen.Id) ?? new UrunDto();
    }

    public async Task<UrunDto?> GuncelleAsync(int id, UrunGuncelleDto dto)
    {
        var urun = await _urunRepository.IdIleGetirAsync(id);
        if (urun == null) return null;

        urun.Ad = dto.Ad;
        urun.Aciklama = dto.Aciklama;
        urun.Fiyat = dto.Fiyat;
        urun.StokAdedi = dto.StokAdedi;
        urun.ResimUrl = dto.ResimUrl;
        urun.AktifMi = dto.AktifMi;
        urun.KategoriId = dto.KategoriId;

        await _urunRepository.GuncelleAsync(urun);
        return await IdIleGetirAsync(id);
    }

    public async Task<bool> SilAsync(int id)
    {
        var urun = await _urunRepository.IdIleGetirAsync(id);
        if (urun == null) return false;

        await _urunRepository.SilAsync(id);
        return true;
    }
}