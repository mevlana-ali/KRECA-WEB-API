using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface IKategoriService
{
    Task<List<KategoriDto>> HepsiniGetirAsync();
    Task<KategoriDto?> IdIleGetirAsync(int id);
    Task<KategoriDto> EkleAsync(KategoriOlusturDto dto);
    Task<bool> SilAsync(int id);
}