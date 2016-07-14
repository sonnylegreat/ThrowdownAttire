using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ThrowdownAttire.App_Start;
using ThrowdownAttire.ViewModels;
using ThrowdownAttire.Repositories;
using System.Web.Routing;
using System.Collections.Generic;

namespace ThrowdownAttire.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class AdminController : Controller
    {
        [HttpPost]
        public ActionResult Create(ShirtCreateViewModel model)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }
            if (model.title == null || Globals.Shirts.Exists(x => x.Title.ToLower() == model.title.ToLower()))
            {
                return RedirectToAction("Admin", "Auth", new RouteValueDictionary(
                    new { success = 0, message = "Create shirt failed: please provide a unique title." }));
            }

            var repo = new ShirtRepository();

            try
            {
                var displayBool = bool.Parse(model.display);
                if (model.newseries != null)
                {
                    model.series = model.newseries;
                    var url = repo.uploadSeriesImage(model.series, model.sliderimage);
                    url = displayBool ? url : null;

                    Globals.Types.Add(model.series, url);
                }

                var shirt = repo.Create(model);
                Globals.Shirts.Add(shirt);
                Globals.Shirts.Sort((x, y) => x.Title.CompareTo(y.Title));
                Globals.Types = Globals.Types.OrderBy(x => x.Key.Length).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception e)
            {
                return RedirectToAction("Admin", "Auth", new RouteValueDictionary(
                    new { success = 0, message = "Create shirt failed: " + e.Message }));
            }

            return RedirectToAction("Admin", "Auth", new RouteValueDictionary(
                    new { success = 1, message = model.title + " successfully added to database." }));

        }

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }
            return View(Globals.Shirts.FirstOrDefault(x => x.Id.ToString() == Id));
        }

        [HttpPost]
        public ActionResult Edit(ShirtEditViewModel model)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var repo = new ShirtRepository();
            var shirtDoc = repo.UpdateShirt(model);
            var shirt = repo.createShirtFromBson(shirtDoc);
            var oldShirt = repo.FindShirtById(shirt.Id.ToString());

            var i = Globals.Shirts.FindIndex(x => x.Id == shirt.Id);
            Globals.Shirts[i] = shirt;

            if (repo.firstShirtOfType(oldShirt.Type) == null)
            {
                Globals.Types.Remove(oldShirt.Type);
                repo.deleteImage(oldShirt.Type);
            }

            return RedirectToAction("Admin", "Auth");
        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase[] images, string id)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var repo = new ShirtRepository();
            var shirt = repo.FindShirtById(id);

            var urls = repo.uploadImages(images, shirt.Title);

            var photoList = shirt.Photos.ToList();
            photoList.AddRange(urls);
            shirt.Photos = photoList.ToArray();

            repo.UpdateShirt(shirt);

            return new JsonResult() { Data = new { sources = urls } };
        }

        [HttpPost]
        public ActionResult DeletePhoto(string src, string id)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var repo = new ShirtRepository();
            var shirt = repo.FindShirtById(id);

            var pub_id = src.Split('/').Last().Replace(".png", "");

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
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var repo = new ShirtRepository();
            var shirt = repo.deleteShirt(id);

            Globals.Shirts.Remove(shirt);

            if (repo.firstShirtOfType(shirt.Type) == null)
            {
                Globals.Types.Remove(shirt.Type);
                repo.deleteImage(shirt.Type);
            }
            else if (repo.firstDisplayedShirtOfType(shirt.Type) == null)
            {
                Globals.Types[shirt.Type] = null;
            }

            return RedirectToAction("Admin", "Auth");
        }

        [HttpPost]
        public ActionResult SetDisplay(string id, string display)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var repo = new ShirtRepository();

            repo.UpdateShirt(id, "display", display);
            var shirt = repo.FindShirtById(id);

            shirt.Display = bool.Parse(display);

            Globals.Types[shirt.Type] = 
                repo.firstDisplayedShirtOfType(shirt.Type) == null ? 
                null : "http://res.cloudinary.com/throw-down-attire/image/upload/" + shirt.Type + ".png";

            return RedirectToAction("Admin", "Auth");
        }

        [HttpPost]
        public ActionResult UpdateSlider(string type, HttpPostedFileBase image)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var repo = new ShirtRepository();
            var src = repo.updateSeriesImage(type, image);

            return new JsonResult() { Data = new { src = src } };
        }

        [HttpPost]
        public ActionResult FAQ(string json)
        {
            if (!Authenticated())
            {
                return RedirectToAction("AdminLogin", "Auth");
            }

            var repo = new ShirtRepository();

            var faqs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Globals.FAQs = faqs;

            repo.saveFAQs();

            return RedirectToAction("Admin", "Auth");
        }

        private bool Authenticated()
        {
            return (bool)Session["Authenticated"];
        }
    }
}