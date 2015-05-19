using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cooking.Data;
using Cooking.Data.Models;
using Cooking.Models;

namespace Cooking.Controllers
{
    public class CategoryController : ControllerBase
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create(CreateCategoryViewModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var category = new Category(categoryModel.Name);
                db.Create(category);
                await db.SaveChangesAsync();

                return new JsonResult()
                {
                    Data = new
                    {
                        success = true,
                        id = category.Id,
                        name = category.Name
                    }
                };
            }

            return new JsonResult()
            {
                Data = new
                {
                    success = false,
                    content = RenderPartialViewToString("_Create", categoryModel)
                }
            };
        }
    }
}