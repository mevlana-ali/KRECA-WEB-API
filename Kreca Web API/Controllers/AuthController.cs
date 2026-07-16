using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KRecaWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [EnableRateLimiting("LoginPolicy")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        var token = _authService.Login(dto);
        if (token == null)
            return Unauthorized("Kullanıcı adı veya şifre hatalı.");

        return Ok(token);
    }
}