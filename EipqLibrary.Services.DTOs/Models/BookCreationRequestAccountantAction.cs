using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.Models
{
    public class BookCreationRequestAccountantAction
    {
        public int RequestId { get; set; }
        public string AccountantMessage { get; set; }
        public BookCreationRequestStatus AccountantActionResult { get; set; }
    }
}
