using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class BookCreationRequest
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public int PagesCount { get; set; }
        public int TotalCount { get; set; }
        public BookStatus Status { get; set; }

        public int CategoryId { get; set; }
    }
}
