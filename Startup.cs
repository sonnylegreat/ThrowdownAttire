using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using ThrowdownAttire.Models;
using ThrowdownAttire.App_Start;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(ThrowdownAttire.Startup))]

namespace ThrowdownAttire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            initializeData().Wait();

            ConfigureAuth(app);
        }

        private async Task initializeData()
        {
            var db = new DBContext().GetDatabase();
            var collection = db.GetCollection<BsonDocument>("Shirts");

            var docs = await collection.Find(new BsonDocument()).ToListAsync();

            foreach(var doc in docs)
            {
                Globals.Shirts.Add(createShirtFromBson(doc));
            }
        }

        private Shirt createShirtFromBson(BsonDocument product)
        {
            var variants = new Dictionary<string, ObjectId>();

            foreach (var variant in new String[] { "XS", "S", "M", "L", "XL" })
            {
                variants.Add(variant, product["variants"].AsBsonArray.First(x => x.AsBsonDocument.Contains(variant))[variant].AsObjectId);
            }

            return new Shirt()
            {
                Id = product["id"].AsObjectId,
                Title = product["title"].AsString,
                Description = product["description"].AsString,
                Photos = product["images"].AsBsonArray.Select(x => x.ToString()).ToArray(),
                Handle = product["handle"].AsString,
                Price = product["price"].AsDouble,
                Stock = product["stock"].AsInt32,
                Type = product["type"].AsString,
                Variants = variants
            };
        }
    }
}
