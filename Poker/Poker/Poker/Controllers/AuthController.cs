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
using Shared.Models.DTOs;
using Poker.Models;
using Poker.Services;
using Shared.Models.DTOs;
using Shared.Models.Utility;

namespace Poker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                string error = "invalid_data";
                if (ModelState.ContainsKey(nameof(request.Name)) && ModelState[nameof(request.Name)].Errors.Any())
                    error = "name_required";
                else if (ModelState.ContainsKey(nameof(request.Password)) && ModelState[nameof(request.Password)].Errors.Any())
                    error = "password_required";

                return Redirect($"/login?error={error}&name={Uri.EscapeDataString(request.Name ?? "")}");
            }

            var response = await authService.LoginAsync(request);

            if (response is null)
            {
                return Redirect($"/login?error=invalid_credentials&name={Uri.EscapeDataString(request.Name)}");
            }

            SetTokenCookies(response.AccessToken, response.RefreshToken);
            return LocalRedirect("/");
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
        public async Task<IActionResult> RefreshToken (RefreshTokenRequestDto request)
        {
            var response = await authService.RefreshTokensAsync(request);
            
            if(response is null)
            {
                return Unauthorized("Invalid refresh token");
            }

            SetTokenCookies(response.AccessToken, response.RefreshToken);

            return Ok();
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

        private void SetTokenCookies(string accessToken, string refreshToken)
        {
            var accessOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            var refreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("accessToken", accessToken, accessOptions);
            Response.Cookies.Append("refreshToken", refreshToken, refreshOptions);
        }
    }
}
