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

        public IQueryable<Recipe> GetRecipes(string categoryName = null)
        {
            if (categoryName != null)
                return this.Recipes.Where(r => r.Categories.Any(c => c.Name == categoryName));
            else
                return this.Recipes;
        }

        public void Delete(Recipe recipe)
        {
            this.Images.Remove(recipe.Image);
            this.Ingredients.RemoveRange(recipe.Ingredients);
            this.Recipes.Remove(recipe);
        }

        public void Create(Image image)
        {
            this.Images.Add(image);
        }

        public Image GetImage(Guid id)
        {
            return this.Images.SingleOrDefault(i => i.Id == id);
        }

        public void Delete(Ingredient ingredient)
        {
            this.Ingredients.Remove(ingredient);
        }

        public void Delete(IEnumerable<Ingredient> ingredient)
        {
            this.Ingredients.RemoveRange(ingredient);
        }

        public async Task<IList<Category>> GetCategoriesAsync()
        {
            var categories = await this.Categories.ToListAsync();
            return categories;
        }

        public Category GetCategory(Guid id)
        {
            var category = this.Categories.Find(id);
            return category;
        }

        public Category GetCategory(string name)
        {
            var category = this.Categories.FirstOrDefault(c => c.Name == name);
            return category;
        }

        public void Create(Category category)
        {
            this.Categories.Add(category);
        }
    }
}