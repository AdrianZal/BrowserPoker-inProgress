using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.DTOs;
using Poker.Models;
using Shared.Models.Utility;

namespace Poker.Services
{
    public class AuthService(PokerContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var player = await context.Players.FirstOrDefaultAsync(p => p.Name == request.Name);
            if (player is null)
            {
                return null;
            }
            if (new PasswordHasher<Player>().VerifyHashedPassword(player, player.Password, request.Password)
                    == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(player);
        }

        public async Task<RegisterResponse?> RegisterAsync(RegisterRequestDto request)
        {
            var errors = new List<RegistrationErrorType>();

            if (await context.Players.AnyAsync(p => p.Name == request.Name))
            {
                errors.Add(RegistrationErrorType.NameInUse);
            }

            if (await context.Players.AnyAsync(p => p.Email.ToLower() == request.Email.ToLower()))
            {
                errors.Add(RegistrationErrorType.EmailInUse);
            }

            if (errors.Count > 0)
            {
                return RegisterResponse.Failure(errors);
            }

            var hashedPassword = new PasswordHasher<Player>()
                .HashPassword(null, request.Password);

            var player = new Player
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword,
                Balance = 10000
            };

            context.Players.Add(player);

            await context.SaveChangesAsync();

            var response = new RegisterResponseDto
            {
                Id = player.Id,
                Name = player.Name,
                Email = player.Email,
                Balance = player.Balance
            };

            return RegisterResponse.Success(response);
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var player = await ValidateRefreshTokenAsync(request.Id, request.RefreshToken);
            if (player is null)
            {
                return null;
            }

            return await CreateTokenResponse(player);
        }

        public async Task LogoutAsync(int id)
        {
            await context.RefreshTokens
                .Where(t => t.PlayerId == id && t.Revoked == false)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Revoked, true));
        }

        private async Task<TokenResponseDto> CreateTokenResponse(Player player)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(player),
                RefreshToken = await CreateAndSaveRefreshToken(player)
            };
        }

        private async Task<Player?> ValidateRefreshTokenAsync(int id, string refreshToken)
        {
            var player = await context.Players.FindAsync(id);
            if (player is null)
            {
                return null;
            }

            var tokens = await context.RefreshTokens
                .Where(t => t.PlayerId == id 
                    && t.Revoked == false
                    && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in tokens)
            {
                if (new PasswordHasher<Player>().VerifyHashedPassword(player, token.TokenHash, refreshToken)
                    == PasswordVerificationResult.Success)
                {
                    return player;
                }
            }

            return null;
        }
        private string CreateToken(Player player)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, player.Name),
                new Claim(ClaimTypes.NameIdentifier, player.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> CreateAndSaveRefreshToken(Player player)
        {
            var tokens = await context.RefreshTokens
                .Where(t => t.PlayerId == player.Id
                    && t.ExpiresAt > DateTime.UtcNow
                    && t.Revoked == false)
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.Revoked, true));

            var token = CreateRefreshToken();
            var hashedToken = new PasswordHasher<Player>()
                .HashPassword(null, token);

            var refreshToken = new RefreshToken {
                PlayerId = player.Id,
                TokenHash = hashedToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                Revoked = false
            };

            context.RefreshTokens.Add(refreshToken);
            await context.SaveChangesAsync();

            return token;
        }

        
    }

}
