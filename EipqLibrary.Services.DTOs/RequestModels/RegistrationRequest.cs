using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class RegistrationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 1)]
        public string LastName { get; set; }

        public string GroupNumber { get; set; }
        public int? GroupCreationYear { get; set; }

        public string PhoneNumber { get; set; }
        public string StudentCardNumber { get; set; }
    }
}
