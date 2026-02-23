using System.ComponentModel.DataAnnotations;

namespace Shared.Models.DTOs
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(16, ErrorMessage = "Name cannot exceed 16 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
        public string Password { get; set; } = string.Empty;

    }
}
