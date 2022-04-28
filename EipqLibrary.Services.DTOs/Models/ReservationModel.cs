using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string ExpectedBorrowingDate { get; set; }
        public string ExpectedReturnDate { get; set; }

        public DateTime? CancellationDate { get; set; }
        public DateTime? ActualBorrowingDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        public ReservationStatus Status { get; set; }

        public string BookAuthor { get; set; }
        public string BookName { get; set; }

        public BookModel Book { get; set; }

        public UserModel User { get; set; }
    }
}
