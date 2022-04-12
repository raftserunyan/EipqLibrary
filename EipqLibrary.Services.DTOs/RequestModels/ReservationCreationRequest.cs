using EipqLibrary.Services.DTOs.ValidationAttributes;
using EipqLibrary.Shared.Utils.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    [ReturnDateIsNotBeforeBorrowingDate]
    public class ReservationCreationRequest
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        [IsNotInPast]
        public DateTime BorrowingDate { get; set; }
        [IsNotInPast]
        public DateTime ReturnDate { get; set; }
    }
}
