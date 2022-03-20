using System;

namespace EipqLibrary.Services.DTOs.Models.Tokens
{
    public class PublicRefreshTokenDto
    {
        public string AccessTokenId { get; set; }
        public string OldAccessTokenId { get; set; }
        public string DeviceId { get; set; }
        public string OldRefreshToken { get; set; }
        public string UserId { get; set; }
    }
}
