using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Poker.Models;
using Poker.Services;
using Shared.Models.DTOs;
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

            await SetTokenCookie(response);

            return LocalRedirect("/menu");
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
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var request = new RefreshTokenRequestDto
            {
                RefreshToken = refreshToken
            };

            var response = await authService.RefreshTokensAsync(request);

            if (response is null)
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken");
                return Unauthorized();
            }

            await SetTokenCookie(response);

            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int id))
            {
                await authService.LogoutAsync(id);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Cookies.Delete("refreshToken");

            return LocalRedirect("/");
        }

        private async Task SetTokenCookie(TokenResponseDto response)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(response.AccessToken);

            var claimsIdentity = new ClaimsIdentity(
                jwtToken.Claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", response.RefreshToken, options);
        }
    }
}
