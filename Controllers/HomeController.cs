using System.Linq;
using System.Web.Mvc;
using ThrowdownAttire.Models;
using ThrowdownAttire.App_Start;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Bson;
using ThrowdownAttire.ViewModels;
using Postal;
using System.Globalization;

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

        public ActionResult Series(string id)
        {
            if (id == null)
            {
                ViewBag.Title = "Series";
                return View(Globals.Types.Where(x => x.Value != null).ToDictionary(x => x.Key, x=> x.Value));
            }

            foreach(var type in Globals.Types.Keys)
            {
                if(id.ToLower() == type.ToLower().Replace(" ", ""))
                {
                    ViewBag.Title = "Series - " + type;
                    return View("Floats", getShirts(type));
                }
            }

            return View(Globals.Shirts);
        }

        public ActionResult Shirt(string id)
        {
            return View(Globals.Shirts.FirstOrDefault(x => x.Handle == id));
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string name, string useremail, string title, string message)
        {
            dynamic email = new Email("Contact");
            email.From = useremail;
            email.Name = name;
            email.Title = title;
            email.Message = message;
            email.Send();
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
            return Globals.Shirts.Where(x => x.Type.ToLower() == type.ToLower() && x.Display && x.Photos.Length > 1);
        }
    }
}
