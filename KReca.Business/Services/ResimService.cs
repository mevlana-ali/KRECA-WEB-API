using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql.BackendMessages;
using System.Security.Principal;

namespace KReca.Business.Services;

public class ResimService : IResimService
{
    private readonly Cloudinary _cloudinary;

    public ResimService(IConfiguration config)
    {
        var account = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<ResimYuklemeDto> YukleAsync(Stream stream, string dosyaAdi)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(dosyaAdi, stream),
            Folder = "kreca-urunler",
            Transformation = new Transformation()
                .Width(800)
                .Height(800)
                .Crop("fill")
                .Quality("auto")
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        return new ResimYuklemeDto
        {
            Url = result.SecureUrl.ToString(),
            PublicId = result.PublicId
        };
    }

    public async Task<bool> SilAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result.Result == "ok";
    }
}