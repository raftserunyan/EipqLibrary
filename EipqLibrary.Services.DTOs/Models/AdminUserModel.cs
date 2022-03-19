using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.Models
{
    public class AdminUserModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Occupation Occupation { get; set; }
    }
}
