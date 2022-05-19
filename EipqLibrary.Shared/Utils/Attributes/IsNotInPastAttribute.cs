using System;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Shared.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsNotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime other)
            {
                if (other >= DateTime.Today)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Դուք չեք կարող նշել անցյալի ամսաթիվ");
        }
    }
}
