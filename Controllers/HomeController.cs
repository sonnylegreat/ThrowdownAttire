using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThrowdownAttire.Models;
using ThrowdownAttire.App_Start;

namespace ThrowdownAttire.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Series()
        {
            ViewBag.Title = "Series";

            return View(Globals.Shirts);
        }

        public ActionResult Shirt(string id)
        {
            return View(Globals.Shirts.FirstOrDefault(x => x.Id == id));
        }
    }
}
