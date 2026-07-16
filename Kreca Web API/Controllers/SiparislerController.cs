using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SiparislerController : ControllerBase
{
    private readonly ISiparisService _siparisService;

    public SiparislerController(ISiparisService siparisService)
    {
        _siparisService = siparisService;
    }

    // GET api/siparisler — Sadece admin
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> HepsiniGetir()
    {
        var siparisler = await _siparisService.HepsiniGetirAsync();
        return Ok(siparisler);
    }

    // GET api/siparisler/5 — Sadece admin (kişisel bilgi içeriyor)
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var siparis = await _siparisService.IdIleGetirAsync(id);
        if (siparis == null) return NotFound();
        return Ok(siparis);
    }

    // POST api/siparisler
    [HttpPost]
    [EnableRateLimiting("OrderPolicy")]
    public async Task<IActionResult> Olustur([FromBody] SiparisOlusturDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var siparis = await _siparisService.OlusturAsync(dto);
            return CreatedAtAction(nameof(IdIleGetir), new { id = siparis.Id }, siparis);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/siparisler/5/durum — Sadece admin
    [Authorize]
    [HttpPut("{id}/durum")]
    public async Task<IActionResult> DurumGuncelle(int id, [FromBody] SiparisDurumGuncelleDto dto)
    {
        var sonuc = await _siparisService.DurumGuncelleAsync(id, dto);
        if (!sonuc) return NotFound();
        return Ok();
    }
}