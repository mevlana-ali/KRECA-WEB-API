using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayTRController : ControllerBase
{
    private readonly IPayTRService _paytrService;

    public PayTRController(IPayTRService paytrService)
    {
        _paytrService = paytrService;
    }

    // POST api/paytr/token-al
    [HttpPost("token-al")]
    public async Task<IActionResult> TokenAl([FromBody] PayTROdemeBaslatDto dto)
    {
        dto.KullaniciIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        var sonuc = await _paytrService.TokenAlAsync(dto);

        if (!sonuc.Basarili)
            return BadRequest(sonuc.Hata);

        return Ok(new { token = sonuc.Token });
    }

    // POST api/paytr/callback
    [HttpPost("callback")]
    public async Task<IActionResult> Callback([FromForm] PayTRCallbackDto dto)
    {
        var sonuc = await _paytrService.CallbackDogrulaAsync(dto);

        // PayTR OK bekleniyor, aksi halde tekrar dener
        if (!sonuc) return BadRequest("PAYTR_HASH_DOGRULAMA_HATASI");

        return Ok("OK");
    }
}