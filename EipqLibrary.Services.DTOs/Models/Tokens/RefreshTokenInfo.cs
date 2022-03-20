using System;

namespace EipqLibrary.Services.DTOs.Models.Tokens
{
    public class RefreshTokenInfo
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
