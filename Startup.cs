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

            var newDocs = createReplaceDocs(Globals.Shirts);

            changeData(newDocs).Wait();

            ConfigureAuth(app);
        }

        private async Task changeData(BsonDocument[] newDocs)
        {
            var db = new DBContext().GetDatabase();
            var collection = db.GetCollection<BsonDocument>("Shirts");

            await collection.InsertManyAsync(newDocs);
        }

        private async Task initializeData()
        {
            var db = new DBContext().GetDatabase();
            var collection = db.GetCollection<BsonDocument>("Shirts");

            var bson = await collection.Find(new BsonDocument() { { "_id", Globals.ShirtDocumentId } }).SingleAsync();

            Globals.Shirts = createShirtsFromBson(bson);
        }

        private BsonDocument[] createReplaceDocs(List<Shirt> shirts)
        {
            var docs = new BsonDocument[shirts.Count];
            for(int i = 0; i < shirts.Count; i++)
            {
                var shirt = shirts.ElementAt(i);
                var doc = new BsonDocument()
                {
                    {"id", shirt.Id },
                    {"title", shirt.Title },
                    {"handle", shirt.Handle },
                    {"type", shirt.Type },
                    {"price", shirt.Price },
                    {"stock", shirt.Stock },
                    {"description", shirt.Description },
                    {"images", new BsonArray(shirt.Photos) },
                    {"variants", new BsonArray(shirt.Variants.Select(x => new BsonDocument(x.Key, x.Value)))}
                };

                docs[i] = doc;
            }
            return docs;
        }

        private List<Shirt> createShirtsFromBson(BsonDocument bson)
        {
            var shirts = new List<Shirt>();

            foreach (var product in bson["products"].AsBsonArray)
            {
                var variants = new Dictionary<string, ObjectId>();

                foreach (var variant in product["variants"].AsBsonArray)
                {
                    variants.Add(variant["size"].AsString, variant["id"].AsObjectId);
                }

                shirts.Add(new Models.Shirt()
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
                });
            }

            return shirts;
        }
    }
}
