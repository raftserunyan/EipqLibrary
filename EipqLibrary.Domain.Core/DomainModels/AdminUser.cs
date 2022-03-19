using EipqLibrary.Domain.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class AdminUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Occupation Occupation { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
