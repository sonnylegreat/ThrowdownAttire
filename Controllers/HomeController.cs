using System.Linq;
using System.Web.Mvc;
using ThrowdownAttire.Models;
using ThrowdownAttire.App_Start;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ThrowdownAttire.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            if(Session != null)
            {
                Session["Authenticated"] = false;
            }
        }

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
            return View(Globals.Shirts.FirstOrDefault(x => x.Id.ToString() == id));
        }

        public ActionResult Drugs()
        {
            ViewBag.Title = "Drugs";
            return View("Floats", getShirts("Drugs"));
        }

        public ActionResult Quote()
        {
            ViewBag.Title = "Quote";
            return View("Floats", getShirts("Quote"));
        }

        public ActionResult Lifestyle()
        {
            ViewBag.Title = "Lifestyle";
            return View("Floats", getShirts("Lifestyle"));
        }

        public ActionResult FPP()
        {
            ViewBag.Title = "FPP";
            return View("Floats", getShirts("FPP"));
        }

        private IEnumerable<Shirt> getShirts(string type)
        {
            return Globals.Shirts.Where(x => x.Type == type);
        }
    }
}
