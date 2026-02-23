using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(16, ErrorMessage = "Name cannot exceed 16 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
