using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class BookCreationRequest
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public int PagesCount { get; set; }
        public int TotalCount { get; set; }

        [Required]
        [IsEnum(typeof(BookStatus))]
        public BookStatus Status { get; set; }

        public int CategoryId { get; set; }
    }
}
