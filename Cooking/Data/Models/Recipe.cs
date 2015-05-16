using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cooking.Data.Models
{
    public class Recipe
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(256), Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string PrepareInstructions { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        public virtual Image Image { get; set; }

        public virtual IList<Ingredient> Ingredients { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public virtual IList<ApplicationUser> Users { get; set; }

        public Recipe()
        {
            this.Id = Guid.NewGuid();
            this.CreateDate = DateTime.UtcNow;
        }
    }
}