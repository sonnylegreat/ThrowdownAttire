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
using ThrowdownAttire.Repositories;

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
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195447/baked_as_cunt.png",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195447/baked_as_cunt_2.png"
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
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st.png",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st_2.png",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st_3.png",
                        "http://res.cloudinary.com/throw-down-attire/image/upload/v1449195438/bond_st_4.png"
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

            var repo = new ShirtRepository();
            var docs = await repo.collection.Find(new BsonDocument()).ToListAsync();

            foreach(var doc in docs)
            {
                var shirt = repo.createShirtFromBson(doc);
                if (!Globals.Types.Keys.Contains(shirt.Type) || (Globals.Types[shirt.Type] == null && shirt.Display == true))
                {
                    var url = shirt.Display ? 
                        "http://res.cloudinary.com/throw-down-attire/image/upload/" + shirt.Type + ".png" : null;
                    Globals.Types.Add(shirt.Type, url);
                }

                Globals.Shirts.Add(shirt);
            }

            Globals.Shirts.Sort((x, y) => x.Title.CompareTo(y.Title));
            Globals.Types = Globals.Types.OrderBy(x => x.Key.Length).ToDictionary(x => x.Key, x => x.Value);

            var faqDocs = await repo.FAQCollection.Find(new BsonDocument()).ToListAsync();

            foreach (var doc in faqDocs)
            {
                repo.createFAQFromBson(doc);
            }
        }
    }
}
