using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using KReca.Data.Entities;
using KReca.Data.Interfaces;

namespace KReca.Business.Services;

public class IletisimMesajiService : IIletisimMesajiService
{
    private readonly IIletisimMesajiRepository _repo;

    public IletisimMesajiService(IIletisimMesajiRepository repo)
    {
        _repo = repo;
    }

    public async Task<IletisimMesajiDto> OlusturAsync(IletisimMesajiOlusturDto dto)
    {
        var mesaj = new IletisimMesaji
        {
            AdSoyad = dto.AdSoyad,
            Email = dto.Email,
            Telefon = dto.Telefon,
            Mesaj = dto.Mesaj
        };

        var eklenen = await _repo.EkleAsync(mesaj);
        return ToDto(eklenen);
    }

    public async Task<List<IletisimMesajiDto>> HepsiniGetirAsync()
    {
        var mesajlar = await _repo.HepsiniGetirAsync();
        return mesajlar
            .OrderByDescending(m => m.OlusturulmaTarihi)
            .Select(ToDto)
            .ToList();
    }

    public async Task<bool> OkunduIsaretle(int id)
    {
        var mesaj = await _repo.IdIleGetirAsync(id);
        if (mesaj == null) return false;

        mesaj.Okundu = true;
        await _repo.GuncelleAsync(mesaj);
        return true;
    }

    private IletisimMesajiDto ToDto(IletisimMesaji m) => new()
    {
        Id = m.Id,
        AdSoyad = m.AdSoyad,
        Email = m.Email,
        Telefon = m.Telefon,
        Mesaj = m.Mesaj,
        Okundu = m.Okundu,
        OlusturulmaTarihi = m.OlusturulmaTarihi
    };
}
