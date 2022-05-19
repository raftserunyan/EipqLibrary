using EipqLibrary.Shared.CommonInterfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Shared.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QuantityEqualsBorrowableAndLibraryOnlySumAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ICountable other)
            {
                if (other.AvailableForBorrowingCount + other.AvailableForUsingInLibraryCount == other.Quantity)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("'Հասանելի տանելու համար' և 'Հասանելի գրադարանում կարդալու համար' դաշտերի գումարը պետք է հավասար լինի ընդհանուր քանակին");
        }
    }
}
