using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
