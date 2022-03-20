using System;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class AdminRefreshToken
    {
        [Key]
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
        public string UserId { get; set; }
        public AdminUser User { get; set; }
    }
}
