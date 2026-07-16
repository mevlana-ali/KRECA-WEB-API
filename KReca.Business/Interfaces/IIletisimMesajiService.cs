using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface IIletisimMesajiService
{
    Task<IletisimMesajiDto> OlusturAsync(IletisimMesajiOlusturDto dto);
    Task<List<IletisimMesajiDto>> HepsiniGetirAsync();
    Task<bool> OkunduIsaretle(int id);
}
