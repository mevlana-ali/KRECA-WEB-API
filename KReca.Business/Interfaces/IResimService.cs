using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface IResimService
{
    Task<ResimYuklemeDto> YukleAsync(Stream stream, string dosyaAdi);
    Task<bool> SilAsync(string publicId);
}