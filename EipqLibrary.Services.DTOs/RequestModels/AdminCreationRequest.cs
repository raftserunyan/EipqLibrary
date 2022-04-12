using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class AdminCreationRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Occupation Occupation { get; set; }
    }
}
