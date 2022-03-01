using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class UpdateUserStatusRequest
    {
        public string Id { get; set; }

        public UserStatus Status { get; set; }
    }
}
