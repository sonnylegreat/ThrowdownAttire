using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using ThrowdownAttire.Models;
using System.Net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using ThrowdownAttire.App_Start;
using Newtonsoft.Json.Linq;

[assembly: OwinStartup(typeof(ThrowdownAttire.Startup))]

namespace ThrowdownAttire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["URL"] + "products.json?fields=id,images,title,body_html,variants,tags,handle");

            request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["APIKey"], ConfigurationManager.AppSettings["Password"]);

            var reader = new StreamReader(request.GetResponse().GetResponseStream());

            var responseString = reader.ReadToEnd();

            dynamic json = JsonConvert.DeserializeObject(responseString);

            Globals.Shirts = createShirtsFromJson(json);

            ConfigureAuth(app);
        }

        private List<Shirt> createShirtsFromJson(dynamic json)
        {
            var shirts = new List<Shirt>();

            foreach (var product in json.products)
            {
                var sizes = new Dictionary<string, string>();

                foreach(var variant in product.variants)
                {
                    sizes.Add((string) variant.title, (string) variant.id);
                }

                var images = (JArray) product.images;

                shirts.Add(new Models.Shirt()
                {
                    Id = product.id,
                    Title = product.title,
                    Description = product.html_body ?? product.tags,
                    Photos = (String[]) images.Values("src").Select(x => x.ToString()).ToArray(),
                    Handle = product.handle,
                    Price = product.variants[0].price,
                    Stock = product.variants[0].inventory_quantity,
                    Type = product.tags,
                    Sizes = sizes
                });
            }

            return shirts;
        }
    }
}
