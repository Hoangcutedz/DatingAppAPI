using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required] // Specifies that the property is required. (not null)
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
