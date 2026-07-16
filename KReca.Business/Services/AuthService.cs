using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KReca.Business.DTOs;
using KReca.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace KReca.Business.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public TokenDto? Login(LoginDto dto)
    {
        var adminUsername = _config["Admin:Username"];
        var adminPassword = _config["Admin:Password"];

        if (dto.Username != adminUsername || dto.Password != adminPassword)
            return null;

        var secret = _config["JWT:Secret"]!;
        var issuer = _config["JWT:Issuer"]!;
        var audience = _config["JWT:Audience"]!;
        var expiry = DateTime.UtcNow.AddDays(int.Parse(_config["JWT:ExpiryDays"]!));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, dto.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiry = expiry
        };
    }
}