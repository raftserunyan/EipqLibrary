using System;

namespace EipqLibrary.Shared.Models
{
    public class TokenSettings
    {
        public int AccessTokenLifetimeInMinutes { get; set; }
        public int AdminRefreshTokenLifetimeInDays { get; set; }
        public int UserRefreshTokenLifetimeInDays { get; set; }
    }
}
