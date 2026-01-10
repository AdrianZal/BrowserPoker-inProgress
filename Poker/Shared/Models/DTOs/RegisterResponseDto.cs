namespace Shared.Models.DTOs
{
    public class RegisterResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Balance { get; set; }
    }
}
