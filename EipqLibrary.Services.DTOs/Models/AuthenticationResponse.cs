using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
        public string UserFirstName { get; set; }
        public string DisplayName { get; set; }
    }
}
