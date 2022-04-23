using EipqLibrary.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class UpdateBookDeletionRequest
    {
        [Required]
        public int RequestId { get; set; }
        [Required]
        public DeletionReason DeletionReason { get; set; }
        public string Note { get; set; }
    }
}
