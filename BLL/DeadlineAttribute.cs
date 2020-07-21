using System;
using System.ComponentModel.DataAnnotations;

namespace BLL
{
    class DeadlineAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => "The deadline must be a date after today's date.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var deadlineDate = ((DateTime)value).Date;

            if (deadlineDate > DateTime.MinValue && deadlineDate <= DateTime.Today)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
