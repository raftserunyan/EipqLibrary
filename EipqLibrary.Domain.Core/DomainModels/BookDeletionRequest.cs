using EipqLibrary.Domain.Core.DomainModels.Common;
using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class BookDeletionRequest : BaseEntity
    {
        public int Count { get; set; }
        public DeletionReason DeletionReason { get; set; }
        public string Note { get; set; }

        public int TemporarelyDeletedBorrowableBooksCount { get; set; }
        public DateTime RequestCreationDate { get; set; }

        public DateTime? AccountantActionDate { get; set; }
        public string AccountantNote { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
