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
        public async Task<ActionResult> Index()
        {
            return View(await db.Recipes.ToListAsync());
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
            return View(recipe);
        }

        // GET: Recipe/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
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
                    Image = db.GetImage(recipeModel.ImageId)
                };

                db.Create(recipe);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

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
                ImageUrl = Url.Content(recipe.Image.ImageUrl)
            };

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

                this.EditIngredients(recipeModel, recipe);
                
                db.Entry(recipe).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
    }
}
