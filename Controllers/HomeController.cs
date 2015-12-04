using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThrowdownAttire.Models;
using ThrowdownAttire.App_Start;
using System.Net;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ThrowdownAttire.Controllers
{
    public class HomeController : Controller
    {
        public IMongoDatabase db = new DBContext().GetDatabase();

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

        [HttpPost]
        public ActionResult AddToCart(string quantity, string id)
        {
            var wc = new WebClient();
            wc.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["APIKey"], ConfigurationManager.AppSettings["Password"]);

            var response = wc.UploadValues("https://throwdown-attire.myshopify.com/admin/cart/add", new NameValueCollection()
            {
                {"quantity", quantity },
                {"id", id }
            });

            var responseStr = Encoding.UTF8.GetString(response);

            return new HttpStatusCodeResult(200);
        }
    }
}
