using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class GetUserRoleRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
