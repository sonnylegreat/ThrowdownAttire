using System.Linq;
using System.Web.Mvc;
using ThrowdownAttire.Models;
using ThrowdownAttire.App_Start;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using ThrowdownAttire.ViewModels;

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

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact";
            return View();
        }

        [HttpGet]
        public ActionResult Variant(string id)
        {
            var oid = new ObjectId(id);

            var model = new ShirtVariantViewModel();
            model.Shirt = Globals.Shirts.FirstOrDefault(x => x.Variants.Values.Contains(oid));
            model.Variant = model.Shirt.Variants.FirstOrDefault(x => x.Value == oid).Key;

            return View(model);
        }

        private IEnumerable<Shirt> getShirts(string type)
        {
            return Globals.Shirts.Where(x => x.Type == type);
        }
    }
}
