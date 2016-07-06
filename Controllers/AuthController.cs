using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThrowdownAttire.App_Start;
using ThrowdownAttire.ViewModels;

namespace ThrowdownAttire.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }

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
        public ActionResult Admin(int success = -1, string message = null)
        {
            if (Session["Authenticated"] != null && (bool)Session["Authenticated"] == true)
            {
                var model = new ShirtCreateViewViewModel
                {
                    shirts = Globals.Shirts,
                    shirtCreateModel = new ShirtCreateViewModel()
                };

                if (success == 0)
                {
                    ViewBag.Failure = message;
                }
                if (success == 1)
                {
                    ViewBag.Success = message;
                }

                return View(model);
            }
            return RedirectToAction("AdminLogin");
        }
    }
}