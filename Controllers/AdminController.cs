using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThrowdownAttire.App_Start;
using ThrowdownAttire.ViewModels;
using ThrowdownAttire.Repositories;
using MongoDB.Bson;

namespace ThrowdownAttire.Controllers
{
    public class AdminController : Controller
    {
        [HttpPost]
        public ActionResult Admin(string username, string password)
        {
            if (username.ToLower() == Globals.Username.ToLower() && password == Globals.Password)
            {
                Session["Authenticated"] = true;

                var model = new ShirtCreateViewViewModel
                {
                    shirts = Globals.Shirts,
                    shirtCreateModel = new ShirtCreateViewModel()
                };

                return View(model);
            }
            return RedirectToAction("AdminLogin");
        }

        [HttpGet]
        public ActionResult Admin()
        {
            if ((bool) Session["Authenticated"] == true)
            {
                var model = new ShirtCreateViewViewModel
                {
                    shirts = Globals.Shirts,
                    shirtCreateModel = new ShirtCreateViewModel()
                };

                return View(model);
            }
            return RedirectToAction("AdminLogin");
        }

        [HttpPost]
        public ActionResult Create(ShirtCreateViewModel model)
        {
            if ((bool)Session["Authenticated"] == true)
            {
                var adminModel = new ShirtCreateViewViewModel
                {
                    shirts = Globals.Shirts,
                    shirtCreateModel = new ShirtCreateViewModel()
                };

                ShirtRepository repo = new ShirtRepository();

                try {
                    repo.uploadImages(model.images, model.title);
                    //repo.Create(model).Wait();
                }
                catch(Exception e)
                {
                    ViewBag.Failure = "Create shirt failed: " + e.Message;
                    return View("Admin", adminModel);
                }

                ViewBag.Success = model.title + "successfully added to database.";
                return View("Admin", adminModel);
            }
            return RedirectToAction("AdminLogin");
        }

        [HttpGet]
        public ActionResult Edit(ObjectId Id)
        {
            if ((bool)Session["Authenticated"] == true)
            {
                return View(Globals.Shirts.FirstOrDefault( x => x.Id == Id));
            }
            return RedirectToAction("AdminLogin");
        }

        public ActionResult AdminLogin()
        {
            return View();
        }
    }
}