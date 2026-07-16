using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KategorilerController : ControllerBase
{
    private readonly IKategoriService _kategoriService;

    public KategorilerController(IKategoriService kategoriService)
    {
        _kategoriService = kategoriService;
    }

    // GET api/kategoriler
    [HttpGet]
    public async Task<IActionResult> HepsiniGetir()
    {
        var kategoriler = await _kategoriService.HepsiniGetirAsync();
        return Ok(kategoriler);
    }

    // GET api/kategoriler/5
    [HttpGet("{id}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var kategori = await _kategoriService.IdIleGetirAsync(id);
        if (kategori == null) return NotFound();
        return Ok(kategori);
    }

    // POST api/kategoriler
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Ekle([FromBody] KategoriOlusturDto dto)
    {
        var kategori = await _kategoriService.EkleAsync(dto);
        return CreatedAtAction(nameof(IdIleGetir), new { id = kategori.Id }, kategori);
    }

    // DELETE api/kategoriler/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _kategoriService.SilAsync(id);
        if (!sonuc) return NotFound();
        return NoContent();
    }
}