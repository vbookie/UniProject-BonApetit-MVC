using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cooking.Data.Models;

namespace Cooking.Models
{
    public class CreateRecipeViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Prepare Instructions")]
        public string PrepareInstructions { get; set; }

        [GuidRequired]
        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        public string ImageUrl { get; set; }

        [NoEmptyValues]
        public IList<string> Ingredients { get; set; }

        //[Required]
        public IList<string> Categories { get; set; }
    }

    public class NoEmptyValues : ValidationAttribute
    {
        public NoEmptyValues() : base() 
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