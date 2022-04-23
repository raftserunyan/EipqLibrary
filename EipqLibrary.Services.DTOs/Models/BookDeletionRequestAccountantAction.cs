using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.Models
{
    public class BookDeletionRequestAccountantAction
    {
        public int RequestId { get; set; }
        public string AccountantMessage { get; set; }
        public BookDeletionRequestStatus AccountantActionResult { get; set; }
    }
}
