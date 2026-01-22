using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "Player ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID must be a positive integer.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Refresh Token is required.")]
        [MaxLength(64, ErrorMessage = "Token length exceeded.")]
        public required string RefreshToken { get; set; }
    }
}
