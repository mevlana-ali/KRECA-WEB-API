using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrunlerController : ControllerBase
{
    private readonly IUrunService _urunService;

    public UrunlerController(IUrunService urunService)
    {
        _urunService = urunService;
    }

    // GET api/urunler
    [HttpGet]
    public async Task<IActionResult> HepsiniGetir()
    {
        var urunler = await _urunService.HepsiniGetirAsync();
        return Ok(urunler);
    }

    // GET api/urunler/5
    [HttpGet("{id}")]
    public async Task<IActionResult> IdIleGetir(int id)
    {
        var urun = await _urunService.IdIleGetirAsync(id);
        if (urun == null) return NotFound();
        return Ok(urun);
    }

    // GET api/urunler/kategori/1
    [HttpGet("kategori/{kategoriId}")]
    public async Task<IActionResult> KategoriIleGetir(int kategoriId)
    {
        var urunler = await _urunService.KategoriIleGetirAsync(kategoriId);
        return Ok(urunler);
    }

    // POST api/urunler
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Ekle([FromBody] UrunOlusturDto dto)
    {
        var urun = await _urunService.EkleAsync(dto);
        return CreatedAtAction(nameof(IdIleGetir), new { id = urun.Id }, urun);
    }

    // PUT api/urunler/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] UrunGuncelleDto dto)
    {
        var urun = await _urunService.GuncelleAsync(id, dto);
        if (urun == null) return NotFound();
        return Ok(urun);
    }

    // DELETE api/urunler/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sonuc = await _urunService.SilAsync(id);
        if (!sonuc) return NotFound();
        return NoContent();
    }
}