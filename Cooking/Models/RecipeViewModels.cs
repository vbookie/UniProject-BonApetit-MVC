using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooking.Models
{
    public class CreateRecipeViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Prepare Instructions")]
        public string PrepareInstructions { get; set; }

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
}