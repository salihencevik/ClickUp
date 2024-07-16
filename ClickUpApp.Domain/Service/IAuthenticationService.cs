using ClickUpApp.Domain.Dto.Auth; 

namespace ClickUpApp.Domain.Service
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequest request);
        Task<AuthResponseDto> RegisterAsync(RegisterRequest request);
    }
}