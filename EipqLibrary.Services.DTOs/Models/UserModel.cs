using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public UserStatus Status { get; set; }
        public string StudentCardNumber { get; set; }
        public string GroupNumber { get; set; }
        public string Email { get; set; }

        public GroupModel Group { get; set; }
    }
}
