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
using System.Text;
using System.IO;
using System.Web.UI;

namespace Cooking.Controllers
{
    public class ImageController : ControllerBase
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Image/Create
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Create()
        {
            return PartialView("_Create");
        }

        // POST: Image/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Create(ImageViewModel imageModel)
        {
            var validImageTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if (imageModel.ImageUpload == null || imageModel.ImageUpload.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            else if (!validImageTypes.Contains(imageModel.ImageUpload.ContentType))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
            }

            if (ModelState.IsValid)
            {
                var image = new Image()
                {
                    Title = imageModel.Title,
                    AltText = imageModel.AltText,
                    Caption = imageModel.Caption,
                };

                var uploadDir = "~/Content/Uploads";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), imageModel.ImageUpload.FileName);
                var imageUrl = Path.Combine(uploadDir, imageModel.ImageUpload.FileName);
                imageModel.ImageUpload.SaveAs(imagePath);
                image.ImageUrl = imageUrl;
           
                db.Images.Add(image);
                await db.SaveChangesAsync();

                return new JsonResult()
                {
                    Data = new
                    {
                        success = true,
                        id = image.Id,
                        url = Url.Content(image.ImageUrl)
                    }
                };
            }

            return new JsonResult()
            {
                Data = new
                {
                    success = false,
                    content = RenderPartialViewToString("_Create", imageModel)
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
