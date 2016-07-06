using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThrowdownAttire.App_Start;
using ThrowdownAttire.ViewModels;
using ThrowdownAttire.Repositories;
using MongoDB.Bson;
using System.Web.Routing;

namespace ThrowdownAttire.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class AdminController : Controller
    {
        [HttpPost]
        public ActionResult Create(ShirtCreateViewModel model)
        {
            if ((bool)Session["Authenticated"] == true)
            {
                if(model.title == null || Globals.Shirts.Exists(x => x.Title.ToLower() == model.title.ToLower()))
                {
                    return RedirectToAction("Admin", "Auth", new RouteValueDictionary(
                        new {success = 0,  message = "Create shirt failed: please provide a unique title." }));
                }

                var repo = new ShirtRepository();

                try {
                    var shirt = repo.Create(model);
                    Globals.Shirts.Add(shirt);
                    Globals.Shirts.Sort((x, y) => x.Title.CompareTo(y.Title));
                }
                catch(Exception e)
                {
                    return RedirectToAction("Admin", "Auth", new RouteValueDictionary(
                        new { success = 0, message = "Create shirt failed: " + e.Message }));
                }

                return RedirectToAction("Admin", "Auth", new RouteValueDictionary(
                        new { success = 1, message = model.title + " successfully added to database."}));
            }
            return RedirectToAction("AdminLogin", "Auth");
        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            if ((bool)Session["Authenticated"] == true)
            {
                return View(Globals.Shirts.FirstOrDefault( x => x.Id.ToString() == Id));
            }
            return RedirectToAction("AdminLogin", "Auth");
        }

        [HttpPost]
        public ActionResult Edit(ShirtEditViewModel model)
        {
            if ((bool)Session["Authenticated"] == true)
            {
                var repo = new ShirtRepository();
                var shirtDoc = repo.UpdateShirt(model);
                var shirt = repo.createShirtFromBson(shirtDoc);

                var i = Globals.Shirts.FindIndex(x => x.Id == shirt.Id);
                Globals.Shirts[i] = shirt;

                return RedirectToAction("Admin", "Auth");
            }
            return RedirectToAction("AdminLogin", "Auth");
        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase[] images, string id)
        {
            var repo = new ShirtRepository();
            var shirt = repo.FindShirtById(id);

            try
            {
                var urls = repo.uploadImages(images, shirt.Title);

                var photoList = shirt.Photos.ToList();
                photoList.AddRange(urls);
                shirt.Photos = photoList.ToArray();

                repo.UpdateShirt(shirt);

                ViewBag.Success = "Pictures added for shirt: " + shirt.Title;
            }
            catch (Exception e)
            {
                ViewBag.Failure = "Image upload failed " + e.Message;
            }
            return View("Edit", shirt.Id.ToString());
        }

        [HttpPost]
        public ActionResult DeletePhoto(string src, string id)
        {
            var repo = new ShirtRepository();
            var shirt = repo.FindShirtById(id);

            var pub_id = src.Split('/').Last().Replace(".jpg", "");

            try
            {
                repo.deleteImage(pub_id);

                var shirtList = shirt.Photos.ToList();
                shirtList.Remove(src);
                shirt.Photos = shirtList.ToArray();
                repo.UpdateShirt(shirt);
                ViewBag.Success = "Successfully deleted image for: " + shirt.Title;
            }
            catch (Exception e)
            {
                ViewBag.Failure = "Image delete failed " + e.Message;
            }

            return View("Edit", shirt.Id.ToString());
        }

        [HttpPost]
        public ActionResult DeleteShirt(string id)
        {
            var repo = new ShirtRepository();
            repo.deleteShirt(id);

            Globals.Shirts.Remove(repo.FindShirtById(id));

            return RedirectToAction("Admin", "Auth");
        }
    }
}