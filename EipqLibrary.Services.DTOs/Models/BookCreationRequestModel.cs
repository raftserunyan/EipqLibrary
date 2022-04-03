using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using System;

namespace EipqLibrary.Services.DTOs.Models
{
    public class BookCreationRequestModel
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int ProductionYear { get; set; }
        public string Description { get; set; }

        public int? PagesCount { get; set; }
        public int Quantity { get; set; }

        public BookAvailability BookAvailability { get; set; }

        public DateTime RequestCreationDate { get; set; } = DateTime.Now;
        public BookCreationRequestStatus RequestStatus = BookCreationRequestStatus.Pending;
        public string AccountantNote { get; set; }

        public Category Category { get; set; }
    }
}
