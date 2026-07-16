using KReca.Business.DTOs;

namespace KReca.Business.Interfaces;

public interface IPayTRService
{
    Task<PayTRTokenDto> TokenAlAsync(PayTROdemeBaslatDto dto);
    Task<bool> CallbackDogrulaAsync(PayTRCallbackDto callback);
}