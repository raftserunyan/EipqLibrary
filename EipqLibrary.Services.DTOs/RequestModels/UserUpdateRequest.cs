using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class UserUpdateRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentCardNumber { get; set; }
        public UserStatus Status { get; set; }
        public string GroupNumber { get; set; }
    }
}
