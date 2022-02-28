using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class UserCreationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string GroupNumber { get; set; }
        public int? GroupCreationYear { get; set; }

        public string PhoneNumber { get; set; }
        public string StudentCardNumber { get; set; }
    }
}
