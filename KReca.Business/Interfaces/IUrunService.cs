using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface IUrunService
{
    Task<List<UrunDto>> HepsiniGetirAsync();
    Task<UrunDto?> IdIleGetirAsync(int id);
    Task<List<UrunDto>> KategoriIleGetirAsync(int kategoriId);
    Task<UrunDto> EkleAsync(UrunOlusturDto dto);
    Task<UrunDto?> GuncelleAsync(int id, UrunGuncelleDto dto);
    Task<bool> SilAsync(int id);
}