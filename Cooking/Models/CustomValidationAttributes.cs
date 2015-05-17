using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cooking.Models
{
    public class NoEmptyValues : ValidationAttribute
    {
        public NoEmptyValues()
            : base()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var values = value as IList<string>;

            if (values != null && values.Count > 0)
            {
                var allValuesAreValid = values.All(v => !string.IsNullOrWhiteSpace(v));

                if (allValuesAreValid)
                    return ValidationResult.Success;
                else
                    return new ValidationResult(string.Format("Empty values for the {0} are not allowed.", validationContext.DisplayName));
            }
            else
            {
                return new ValidationResult(string.Format("At least 1 entry for {0} is required.", validationContext.DisplayName));
            }
        }
    }

    public class GuidRequired : ValidationAttribute
    {
        public GuidRequired()
            : base("The {0} is required.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Guid guid;
            if (Guid.TryParse(value.ToString(), out guid) && guid != Guid.Empty)
            {
                return ValidationResult.Success;
            }
            else
            {
                var error = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(error);
            }
        }
    }
}