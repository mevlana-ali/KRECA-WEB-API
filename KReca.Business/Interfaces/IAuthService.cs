using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface IAuthService
{
    TokenDto? Login(LoginDto dto);
}