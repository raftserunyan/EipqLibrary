using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class GetUserRoleByTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
