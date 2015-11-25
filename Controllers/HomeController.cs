using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThrowdownAttire.Models;

namespace ThrowdownAttire.Controllers
{
    public class HomeController : Controller
    {
        Guid id = Guid.Parse("f287571b-4fdd-471f-bc20-d40367181bc6");
        Shirt[] shirts = new Shirt[20];

        public ActionResult Index()
        {

            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Series()
        {
            makeShirts();

            ViewBag.Title = "Series";

            return View(shirts);
        }

        public ActionResult Shirt(Guid id)
        {
            makeShirts();

            return View(shirts.FirstOrDefault(x => x.Id == id));
        }

        private void makeShirts()
        {
            for (int i = 0; i < shirts.Length; i++)
            {
                if (i < 7)
                {
                    shirts[i] = new Shirt()
                    {
                        Id = id,
                        Title = "Drugs Shirt " + (i % 4 + 1),
                        Stock = 3,
                        Photo = "Content/logo.png",
                        Price = 4.20,
                        Type = Models.Shirt.ShirtType.Drugs,
                        Description = "This is a Drugs shirt."
                    };

                }
                else if (i < 9)
                {
                    shirts[i] = new Shirt()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Option 2 Shirt " + (i % 4 + 1),
                        Stock = 3,
                        Photo = "Content/SliderImages/bakedAsCunt.jpg",
                        Price = 4.20,
                        Type = Models.Shirt.ShirtType.Option2,
                        Description = "This is an Option 2 shirt."
                    };
                }
                else if (i < 15)
                {
                    shirts[i] = new Shirt()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Lifestyle Shirt " + (i % 4 + 1),
                        Stock = 3,
                        Photo = "Content/logo.png",
                        Price = 4.20,
                        Type = Models.Shirt.ShirtType.Lifestyle,
                        Description = "This is a Lifestyle shirt."
                    };
                }
                else
                {
                    shirts[i] = new Shirt()
                    {
                        Id = Guid.NewGuid(),
                        Title = "FPP Shirt " + (i % 4 + 1),
                        Stock = 3,
                        Photo = "Content/SliderImages/bakedAsCunt.jpg",
                        Price = 4.20,
                        Type = Models.Shirt.ShirtType.Fpp,
                        Description = "This is a Fuck Plain Packaging shirt."
                    };
                }
            }
        }
    }
}
