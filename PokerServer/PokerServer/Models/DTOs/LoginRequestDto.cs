using System.ComponentModel.DataAnnotations;

namespace PokerServer.Models.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
