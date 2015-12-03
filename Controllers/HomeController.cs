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

namespace ThrowdownAttire.Controllers
{
    public class HomeController : Controller
    {
        public IMongoDatabase db = new DBContext().GetDatabase();

        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";

            await insertIntoDB(Globals.Shirts);

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

        private async Task<string> insertIntoDB(List<Shirt> shirts)
        {

            var document = new BsonDocument()
            {
                { "_id", ObjectId.GenerateNewId() }
            };

            var productArray = new BsonArray();

            foreach (Shirt shirt in shirts)
            {
                productArray.Add(new BsonDocument()
                {
                    { "id", shirt.Id },
                    { "title", shirt.Title },
                    { "images", new BsonArray(shirt.Photos) },
                    { "price", shirt.Price },
                    { "stock", shirt.Stock },
                    { "handle", shirt.Handle },
                    { "description", shirt.Description },
                    { "type", shirt.Type },
                    { "variants", new BsonArray(shirt.Variants.Select(x => new BsonDocument()
                        {
                            {"id", x.Key },
                            {"size", x.Value }
                        }))
                    }
                });
            }

            document.AddRange(new BsonDocument() { { "products", productArray } });

            var collection = db.GetCollection<BsonDocument>("Shirts");
            await collection.InsertOneAsync(document);

            return collection.ToJson();
        }
    }
}
