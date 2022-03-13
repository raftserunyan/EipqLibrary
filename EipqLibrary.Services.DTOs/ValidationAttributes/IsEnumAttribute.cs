using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EipqLibrary.Services.DTOs.ValidationAttributes
{
    public class IsEnumAttribute : ValidationAttribute
    {
        private readonly Type _enumType;
        private static string DefaultErrorMessage => $"Invalid type";
        private string FinalErrorMessage => ErrorMessage ?? DefaultErrorMessage;
        private static ValidationResult SuccessValidation => ValidationResult.Success;
        private ValidationResult ErrorValidation => new ValidationResult(FinalErrorMessage);

        public IsEnumAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return IsValidComposedValue(value) ?
                SuccessValidation :
                ErrorValidation;
        }

        private bool IsValidComposedValue(object value)
        {
            if (value is ICollection collection)
            {
                return collection.Cast<object>().All(IsValidValue);
            }

            return IsValidValue(value);
        }

        private bool IsValidValue(object value)
        {
            // Required attribute can be used if not null value is needed
            return value == null || Enum.IsDefined(_enumType, value);
        }
    }
}
