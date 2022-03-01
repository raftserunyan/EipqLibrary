using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Shared.Web.Dtos.Tokens
{
    public class AuthenticationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
