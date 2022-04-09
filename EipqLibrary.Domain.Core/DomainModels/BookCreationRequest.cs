using EipqLibrary.Domain.Core.DomainModels.Common;
using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class BookCreationRequest : BaseEntity
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public string Description { get; set; }

        public int? PagesCount { get; set; }

        public int Quantity { get; set; }
        public int AvailableForBorrowingCount { get; set; }
        public int AvailableForUsingInLibraryCount { get; set; }

        public DateTime RequestCreationDate { get; set; } = DateTime.Now;
        public DateTime RequestLastUpdatedDate{ get; set; } = DateTime.Now;

        public DateTime? AccountantActionDate { get; set; }
        public BookCreationRequestStatus RequestStatus { get; set; } = BookCreationRequestStatus.Pending;
        public string AccountantNote { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
