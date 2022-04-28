using EipqLibrary.Domain.Core.DomainModels.Common;
using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class Reservation : BaseEntity
    {
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime ExpectedBorrowingDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }

        public DateTime? CancellationDate { get; set; }
        public DateTime? ActualBorrowingDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Reserved;

        public string BookName { get; set; }
        public string BookAuthor { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int? BookInstanceId { get; set; }
        public BookInstance BookInstance { get; set; }
    }
}
