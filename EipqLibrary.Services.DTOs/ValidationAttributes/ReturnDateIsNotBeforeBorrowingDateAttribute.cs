using EipqLibrary.Services.DTOs.RequestModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ReturnDateIsNotBeforeBorrowingDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ReservationCreationRequest other)
            {
                if (other.ReturnDate >= other.BorrowingDate)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Վերադաձնելու օրը չի կարող լինել վերցնելու օրվանից ավելի շուտ");
        }
    }
}
