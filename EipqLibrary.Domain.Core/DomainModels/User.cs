using EipqLibrary.Domain.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentCardNumber { get; set; }
        public string GroupNumber { get; set; }
        public int? GroupCreationYear { get; set; }
        public UserStatus Status { get; set; }
    }
}
