using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.Authentication
{
    public class TokenValidation
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
