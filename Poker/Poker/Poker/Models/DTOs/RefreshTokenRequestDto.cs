using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "Refresh Token is required.")]
        [MaxLength(64, ErrorMessage = "Token length exceeded.")]
        public required string RefreshToken { get; set; }
    }
}
