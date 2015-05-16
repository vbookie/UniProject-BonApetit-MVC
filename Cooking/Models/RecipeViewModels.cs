using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public string PrepareInstructions { get; set; }

        [Required]
        public virtual IList<string> Ingredients { get; set; }

        [Required]
        public virtual IList<string> Categories { get; set; }
    }
}