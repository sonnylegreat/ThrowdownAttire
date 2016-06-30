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
            if (Globals.Testing)
            {
                initializeFakeData();
            }
            else
            {
                initializeData().Wait();
            }

            ConfigureAuth(app);
        }

        private void initializeFakeData()
        {
            Globals.Shirts.AddRange(new Shirt[]
            {
                new Shirt()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "Baked As Cunt",
                    Stock = 3,
                    Description = "Drugs",
                    Handle = "baked-as-cunt",
                    Photos = new string[] 
                    {
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195447/baked_as_cunt.jpg",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195447/baked_as_cunt_2.jpg"
                    },
                    Price = 29.99,
                    Type = "Drugs",
                    Variants = new Dictionary<string, ObjectId>()
                    {
                        {"XS", ObjectId.GenerateNewId() },
                        {"S", ObjectId.GenerateNewId() },
                        {"M", ObjectId.GenerateNewId() },
                        {"L", ObjectId.GenerateNewId() },
                        {"XL", ObjectId.GenerateNewId() }
                    }
                },
                new Shirt()
                {
                    Id = ObjectId.GenerateNewId(),
                    Title = "Bond St",
                    Stock = 3,
                    Description = "FPP",
                    Handle = "bond-st",
                    Photos = new string[] 
                    {
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st.jpg",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st_2.jpg",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st_3.jpg",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st_4.jpg"
                    },
                    Price = 29.99,
                    Type = "FPP",
                    Variants = new Dictionary<string, ObjectId>()
                    {
                        {"XS", ObjectId.GenerateNewId() },
                        {"S", ObjectId.GenerateNewId() },
                        {"M", ObjectId.GenerateNewId() },
                        {"L", ObjectId.GenerateNewId() },
                        {"XL", ObjectId.GenerateNewId() }
                    }
                }
            });
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
