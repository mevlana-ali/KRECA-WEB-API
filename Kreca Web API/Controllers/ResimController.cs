using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResimController : ControllerBase
{
    private readonly IResimService _resimService;

    public ResimController(IResimService resimService)
    {
        _resimService = resimService;
    }

    // POST api/resim/yukle
    [HttpPost("yukle")]
    public async Task<IActionResult> Yukle(IFormFile dosya)
    {
        if (dosya == null || dosya.Length == 0)
            return BadRequest("Dosya seçilmedi.");

        var izinliTipler = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
        if (!izinliTipler.Contains(dosya.ContentType.ToLower()))
            return BadRequest("Sadece JPG, PNG ve WebP yükleyebilirsiniz.");

        if (dosya.Length > 5 * 1024 * 1024)
            return BadRequest("Dosya 5MB'dan büyük olamaz.");

        using var stream = dosya.OpenReadStream();
        var sonuc = await _resimService.YukleAsync(stream, dosya.FileName);
        return Ok(sonuc);
    }

    // DELETE api/resim/sil
    [HttpDelete("sil")]
    public async Task<IActionResult> Sil([FromQuery] string publicId)
    {
        if (string.IsNullOrEmpty(publicId))
            return BadRequest("PublicId gerekli.");

        var sonuc = await _resimService.SilAsync(publicId);
        if (!sonuc) return BadRequest("Resim silinemedi.");
        return Ok();
    }
}