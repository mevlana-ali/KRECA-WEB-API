using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IletisimController : ControllerBase
{
    private readonly IIletisimMesajiService _mesajService;

    public IletisimController(IIletisimMesajiService mesajService)
    {
        _mesajService = mesajService;
    }

    // POST api/iletisim — Public: mesaj gönder
    [HttpPost]
    public async Task<IActionResult> Gonder([FromBody] IletisimMesajiOlusturDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var mesaj = await _mesajService.OlusturAsync(dto);
        return Ok(mesaj);
    }

    // GET api/iletisim — Admin: tüm mesajları listele
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> HepsiniGetir()
    {
        var mesajlar = await _mesajService.HepsiniGetirAsync();
        return Ok(mesajlar);
    }

    // PUT api/iletisim/5/okundu — Admin: okundu işaretle
    [Authorize]
    [HttpPut("{id}/okundu")]
    public async Task<IActionResult> OkunduIsaretle(int id)
    {
        var sonuc = await _mesajService.OkunduIsaretle(id);
        if (!sonuc) return NotFound();
        return Ok();
    }
}
