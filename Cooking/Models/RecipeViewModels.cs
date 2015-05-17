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

    public class EditRecipeViewModel
    {
        [GuidRequired]
        public Guid Id { get; set; }

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
}