using System;

namespace EipqLibrary.Shared.Web.Dtos.Tokens
{
    public class TokenInfo
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
