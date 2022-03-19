using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class AdminInfo
    {
        public string Name { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string Email { get; set; } = null;
        public bool IsActive { get; set; }
        public string Occupation { get; set; } = null;
        public string PhoneNumber { get; set; } = null;
    }
}
