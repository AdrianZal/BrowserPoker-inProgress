using PokerServer.Models;
using Shared.Models.DTOs;
using Shared.Models.Utility;

namespace PokerServer.Services
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequestDto request);
        Task<TokenResponseDto?> LoginAsync(LoginRequestDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

        Task LogoutAsync(int id);
    }
}
