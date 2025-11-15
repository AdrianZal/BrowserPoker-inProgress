using PokerServer.Models;
using PokerServer.Models.DTOs;
using PokerServer.Models.Utility;

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
