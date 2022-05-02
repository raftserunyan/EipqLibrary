using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class BookDeletionRequestModel
    {
        public int Id { get; set; }

        public int Count { get; set; }
        public DeletionReason DeletionReason { get; set; }
        public BookDeletionRequestStatus Status { get; set; }
        public string Note { get; set; }

        public string BookName { get; set; }
        public string BookAuthor { get; set; }

        public DateTime RequestCreationDate { get; set; }

        public DateTime? AccountantActionDate { get; set; }
        public string AccountantNote { get; set; }

        public BookModel Book { get; set; }
    }
}
