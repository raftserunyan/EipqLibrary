using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class AdminInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Occupation { get; set; }
        public string PhoneNumber { get; set; }
    }
}
