using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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

        [Required]
        public Guid[] Categories { get; set; }
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

        [Required]
        public Guid[] Categories { get; set; }
    }

    public class DetailsRecipeViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "How to prepare")]
        public string PrepareInstructions { get; set; }

        public string ImageUrl { get; set; }

        public IList<string> Ingredients { get; set; }

        //[Required]
        public IList<string> Categories { get; set; }
    }

    public class IndexRecipeViewModel
    {
        public IEnumerable<SingleRecipeViewModel> Recipes { get; set; }

        public int PageId { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public string CurrentCategory { get; set; }

        public IList<string> Categories { get; set; }
    }

    public class SingleRecipeViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }
}