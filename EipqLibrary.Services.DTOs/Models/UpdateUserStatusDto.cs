using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.Models
{
    public class UpdateUserStatusDto
    {
        public string Id { get; set; }

        public UserStatus Status { get; set; }
    }
}
