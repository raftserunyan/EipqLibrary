using Microsoft.AspNet.Identity.EntityFramework;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentCardNumber { get; set; }
    }
}
