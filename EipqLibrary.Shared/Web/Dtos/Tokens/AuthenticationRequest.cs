using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Shared.Web.Dtos.Tokens
{
    public class AuthenticationRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
