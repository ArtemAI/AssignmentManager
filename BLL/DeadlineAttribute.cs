using System;
using System.ComponentModel.DataAnnotations;

namespace BLL
{
    /// <summary>
    /// Performs validation of provided assignment deadline value.
    /// </summary>
    internal class DeadlineAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var deadlineDate = ((DateTime)value).Date;

            if (deadlineDate > DateTime.MinValue && deadlineDate <= DateTime.Today)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        private static string GetErrorMessage()
        {
            return "The deadline must be a date after today's date.";
        }
    }
}