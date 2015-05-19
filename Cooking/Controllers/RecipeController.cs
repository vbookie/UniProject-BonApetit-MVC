using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cooking.Data;
using Cooking.Data.Models;
using Cooking.Models;

namespace Cooking.Controllers
{
    public class RecipeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipe
        public async Task<ActionResult> Index(int id = 0, string category = null)
        {
            var allRecipes = db.GetRecipes(category).OrderByDescending(r => r.CreateDate);

            var totalRecipesCount = await allRecipes.CountAsync();
            var recipes = await allRecipes.Skip(id * 8).Take(8).ToListAsync();

            var latestRecipes = await GetLatestRecipes();

            var model = new IndexRecipeViewModel()
            {
                PageId = id,
                HasNextPage = (totalRecipesCount / 8) > id,
                HasPreviousPage = id > 0,
                Recipes = recipes.Select(
                    r => new SingleRecipeViewModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ImageUrl = Url.Content(r.Image.ImageUrl)
                    }),
                LatestRecipes = latestRecipes.Select(
                    r => new SingleRecipeViewModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ImageUrl = Url.Content(r.Image.ImageUrl)
                    }),
                CurrentCategory = category,
                Categories = (await db.GetCategoriesAsync()).Select(c => c.Name).OrderBy(c => c).ToList()
            };

            return View(model);
        }

        // GET: Recipe/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            var latestRecipes = await GetLatestRecipes();
            var similarRecipes = GetSimilarRecipes(recipe);

            var model = new DetailsRecipeViewModel()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                PrepareInstructions = recipe.PrepareInstructions,
                Ingredients = recipe.Ingredients.Select(i => i.Content).ToList(),
                ImageUrl = Url.Content(recipe.Image.ImageUrl),
                LatestRecipes = latestRecipes.Select(
                    r => new SingleRecipeViewModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ImageUrl = Url.Content(r.Image.ImageUrl)
                    }),
                SimilarRecipes = similarRecipes.Select(
                    r => new SingleRecipeViewModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        ImageUrl = Url.Content(r.Image.ImageUrl)
                    }),
            };

            return View(model);
        }

        // GET: Recipe/Create
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create()
        {
            await this.LoadCategoriesInViewBag();

            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Administrator")]
        public async Task<ActionResult> Create(CreateRecipeViewModel recipeModel)
        {
            if (ModelState.IsValid)
            {
                var recipe = new Recipe()
                {
                    Name = recipeModel.Name,
                    Description = recipeModel.Description,
                    PrepareInstructions = recipeModel.PrepareInstructions,
                    Ingredients = recipeModel.Ingredients.Select(i => (Ingredient)i).ToList(),
                    Image = db.GetImage(recipeModel.ImageId),
                    Categories = db.Categories.Where(c => recipeModel.Categories.Contains(c.Id)).ToList()
                };

                db.Create(recipe);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            if (recipeModel.ImageId != Guid.Empty)
            {
                recipeModel.ImageUrl = Url.Content(db.GetImage(recipeModel.ImageId).ImageUrl);
            }

            await this.LoadCategoriesInViewBag(recipeModel);

            return View(recipeModel);
        }

        // GET: Recipe/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            var model = new EditRecipeViewModel()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                PrepareInstructions = recipe.PrepareInstructions,
                Ingredients = recipe.Ingredients.Select(i => i.Content).ToList(),
                ImageId = recipe.Image.Id,
                ImageUrl = Url.Content(recipe.Image.ImageUrl),
                Categories = recipe.Categories.Select(c => c.Id).ToArray()
            };

            await this.LoadCategoriesInViewBag(model);

            return View(model);
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Edit(EditRecipeViewModel recipeModel)
        {
            if (ModelState.IsValid)
            {
                var recipe = db.GetRecipe(recipeModel.Id);

                recipe.Name = recipeModel.Name;
                recipe.Description = recipeModel.Description;
                recipe.PrepareInstructions = recipeModel.PrepareInstructions;
                recipe.Image = db.GetImage(recipeModel.ImageId);

                this.EditCategories(recipeModel, recipe);

                this.EditIngredients(recipeModel, recipe);
                
                db.Entry(recipe).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = recipe.Id });
            }

            await this.LoadCategoriesInViewBag(recipeModel);

            if (recipeModel.ImageId != Guid.Empty)
            {
                recipeModel.ImageUrl = Url.Content(db.GetImage(recipeModel.ImageId).ImageUrl);
            }

            return View(recipeModel);
        }

        // GET: Recipe/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = await db.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Recipe recipe = await db.Recipes.FindAsync(id);
            db.Delete(recipe);

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditIngredients(EditRecipeViewModel recipeModel, Recipe recipe)
        {
            var newIngredients = new List<Ingredient>();

            foreach (var ingredientContent in recipeModel.Ingredients)
            {
                Ingredient ingredient;
                var existingIngredient = recipe.Ingredients.FirstOrDefault(i => i.Content == ingredientContent);
                if (existingIngredient == null)
                    ingredient = new Ingredient() { Content = ingredientContent };
                else
                {
                    ingredient = existingIngredient;
                    recipe.Ingredients.Remove(existingIngredient);
                }

                newIngredients.Add(ingredient);
            }

            db.Delete(recipe.Ingredients);

            recipe.Ingredients = newIngredients;
        }

        private void EditCategories(EditRecipeViewModel recipeModel, Recipe recipe)
        {
            var newCategories = new List<Category>();

            foreach (var categoryId in recipeModel.Categories)
            {
                var existing = recipe.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (existing == null)
                    recipe.Categories.Add(db.GetCategory(categoryId));
            }

            var categoriesToRemove = recipe.Categories.Where(c => !recipeModel.Categories.Contains(c.Id)).ToList();
            foreach (var categoryToRemove in categoriesToRemove)           
                recipe.Categories.Remove(categoryToRemove);
        }

        private async Task LoadCategoriesInViewBag(CreateRecipeViewModel recipeModel = null)
        {
            var categories = await db.GetCategoriesAsync();
            ViewBag.CategoriesList = categories
                    .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = recipeModel != null && recipeModel.Categories != null && recipeModel.Categories.Contains(c.Id) });
        }

        private async Task LoadCategoriesInViewBag(EditRecipeViewModel recipeModel)
        {
            var categories = await db.GetCategoriesAsync();
            ViewBag.CategoriesList = categories
                    .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString(), Selected = recipeModel != null && recipeModel.Categories != null && recipeModel.Categories.Contains(c.Id) });
        }

        private async Task<List<Recipe>> GetLatestRecipes()
        {
            var latestRecipes = await db.GetRecipes().OrderByDescending(r => r.CreateDate).Take(3).ToListAsync();
            return latestRecipes;
        }

        private List<Recipe> GetSimilarRecipes(Recipe recipe)
        {
            //List<Recipe> similarRecipes;

            //foreach (var category in recipe.Categories)
            //{
            //    similarRecipes.AddRange(db.GetRecipes(category.Name));
            //}

            //var allRecipes = similarRecipes.Where(r => r.Id != recipe.Id).ToList();
            //// Ignore the current recipe
            //    .Where(r => r.Categories.Any(c => recipe.Categories.Any(rc => rc.Name == c.Name))) // Get recipes which have at least one category the currect recipe has as well
            //    .OrderByDescending(r => r.CreateDate)
            //    .Take(3);

            var allRecipes = db.GetRecipes().ToList();
            var similarRecipes = allRecipes
                .Where(r => r.Id != recipe.Id)
                .Where(r => r.Categories.Any(c => recipe.Categories.Any(rc => rc.Name == c.Name))) // Get recipes which have at least one category the currect recipe has as well
                .OrderByDescending(r => r.CreateDate)
                .Take(3);

            return similarRecipes.ToList();
        }
    }
}
