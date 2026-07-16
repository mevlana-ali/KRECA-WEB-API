using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AyarlarController : ControllerBase
{
    private readonly ISiteAyarlariService _ayarlarService;

    public AyarlarController(ISiteAyarlariService ayarlarService)
    {
        _ayarlarService = ayarlarService;
    }

    // GET api/ayarlar — Admin: tüm ayarlar
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Getir()
    {
        var ayarlar = await _ayarlarService.GetirAsync();
        return Ok(ayarlar);
    }

    // GET api/ayarlar/kargo — Public: sadece kargo bilgisi
    [HttpGet("kargo")]
    public async Task<IActionResult> KargoAyarlari()
    {
        var kargo = await _ayarlarService.KargoAyarlariGetirAsync();
        return Ok(kargo);
    }

    // PUT api/ayarlar — Admin: ayarları güncelle
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Guncelle([FromBody] SiteAyarlariGuncelleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var sonuc = await _ayarlarService.GuncelleAsync(dto);
        return Ok(sonuc);
    }
}
