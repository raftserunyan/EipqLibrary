using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class BookAdditionRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        public string Description { get; set; }

        public int? PagesCount { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [IsEnum(typeof(BookAvailability))]
        public BookAvailability BookAvailability { get; set; }

        public int CategoryId { get; set; }
    }
}
