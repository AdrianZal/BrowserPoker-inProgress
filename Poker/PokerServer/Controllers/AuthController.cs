using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using PokerServer.Models;
using PokerServer.Services;
using Shared.Models.DTOs;
using Shared.Models.Utility;

namespace PokerServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginRequestDto request)
        {
            var response = await authService.LoginAsync(request);

            if (response is null)
            {
                return BadRequest("Invalid name or password");
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register(RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await authService.RegisterAsync(request);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            return BadRequest(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken (RefreshTokenRequestDto request)
        {
            var response = await authService.RefreshTokensAsync(request);
            
            if(response is null)
            {
                return Unauthorized("Invalid refresh token");
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int id))
            {
                return BadRequest("Invalid token");
            }

            await authService.LogoutAsync(id);

            return NoContent();
        }
    }
}
