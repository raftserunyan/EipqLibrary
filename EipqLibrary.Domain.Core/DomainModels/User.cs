using EipqLibrary.Domain.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentCardNumber { get; set; }
        public UserStatus Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string GroupNumber { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public List<Reservation> Borrowings { get; set; }
    }
}
