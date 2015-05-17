using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Cooking.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Cooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public void Create(Recipe recipe)
        {
            this.Recipes.Add(recipe);
        }

        public Recipe GetRecipe(Guid id)
        {
            var recipe = this.Recipes.SingleOrDefault(r => r.Id == id);
            return recipe;
        }

        public void Create(Image image)
        {
            this.Images.Add(image);
        }

        public Image GetImage(Guid id)
        {
            return this.Images.SingleOrDefault(i => i.Id == id);
        }
    }
}