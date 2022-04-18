using EipqLibrary.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class BookDeletionRequestDto
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
        [Required]
        public DeletionReason DeletionReason { get; set; }
        public string Note { get; set; }
    }
}
