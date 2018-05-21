using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using JavaScript.Models;
using System.IO;
using System.Web;
using Microsoft.AspNet.Identity;
using JavaScript.Filters;

namespace JavaScript.Controllers
{
    public class PhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photos  
        public ActionResult Index()
        {
            var photos = db.Photos.ToList();

            return Json(photos, JsonRequestBehavior.AllowGet);
        }

        // GET: Photos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList("Hej", "Id", "Email");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        //Kolla nere vid Edit för den gamla bind koden, använd PhotoViewModel istället för Photo
        public ActionResult Create(string title, string url, int userId, HttpPostedFileBase imageUpload)
        {
            if (ModelState.IsValid)
            {

                //Create new Photo from the information posted.
                var photo = new Photo
                {
                    Title = title,
                    UserId = userId
                };
                
                //Different valide image types, check against these later.
                var imageValidationTypes = new string[]
               {
                    "image/gif",
                    "image/jpeg",
                    "image/jpg",
                    "image/pjpeg",
                    "image/png"
               };

                //If the title is == null, , return JSON with info about the issue.
                if (title == null)
                {
                    return Json("You are lacking a title, please type one!");
                }
                //If the imageUpload is null, return JSON with info about the issue.
                else if(imageUpload == null)
                {
                    return Json("You are lacking an image, please upload one!");
                    
                }
                //Check if the image is a valid content type.
                else if (!imageValidationTypes.Contains(imageUpload.ContentType))
                {
                    return Json("Please upload a GIF, JPEG or PNG file!");
                }
                //Maybe try and add a size check to not have server overload?

                //Checks to see if the image exists.
                else if (imageUpload != null && imageUpload.ContentLength > 0)
                {
                    //The directory where the image lands.
                    var uploadDir = "~/Content/Sample_Images/";
                    //Combines the path where the image lands and the filename. Maybe hash the filename?
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), imageUpload.FileName);
                    //Save the image onto the server.
                    imageUpload.SaveAs(imagePath);
                    //Insert Photo url.
                    photo.Url = url;
                    //Save the Photo onto the database.
                    db.Photos.Add(photo);
                    db.SaveChanges();
                    return Json("Your image has been added!");
                }

                return Json("Something went wrong, please try again!");
            }
            else
            {
                return Json("Something went wrong, please try again!");
            }
            

        }

        // GET: Photos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList("Hej", "Id", "Email", photo.UserId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Url,UserId")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList("Hej", "Id", "Email", photo.UserId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateJsonAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return Json("Your image have been deleted!");
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
