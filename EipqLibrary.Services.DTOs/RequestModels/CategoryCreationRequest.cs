using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class CategoryCreationRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
