using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Shared.Web.Dtos.Tokens
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
