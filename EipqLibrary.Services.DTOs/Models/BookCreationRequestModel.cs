using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class BookCreationRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public string Description { get; set; }

        public int? PagesCount { get; set; }

        public int Quantity { get; set; }
        public int AvailableForBorrowingCount { get; set; }
        public int AvailableForUsingInLibraryCount { get; set; }

        public DateTime RequestCreationDate { get; set; } = DateTime.Now;
        public DateTime RequestLastUpdatedDate { get; set; } = DateTime.Now;
        public DateTime? AccountantActionDate { get; set; }


        public BookCreationRequestStatus RequestStatus { get; set; }
        public string AccountantNote { get; set; }

        public CategoryModel Category { get; set; }
    }
}
