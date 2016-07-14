using MongoDB.Driver;
using System.Linq;
using System.Web;
using ThrowdownAttire.Models;
using ThrowdownAttire.ViewModels;
using MongoDB.Bson;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ThrowdownAttire.App_Start;
using System.IO;
using System;
using System.Collections.Generic;

namespace ThrowdownAttire.Repositories
{
    public class ShirtRepository
    {
        public IMongoCollection<BsonDocument> collection;

        public ShirtRepository()
        {
            var db = new DBContext().GetDatabase();
            collection = db.GetCollection<BsonDocument>("Shirts");
        }

        public Shirt Create(ShirtCreateViewModel model)
        {
            var images = uploadImages(model.images, model.title);

            var document = new BsonDocument()
            {
                {"id", ObjectId.GenerateNewId() },
                {"title", model.title },
                {"handle", model.title.ToLower().Replace(' ', '-') },
                {"type", model.series },
                {"price", model.price },
                {"stock", 4 },
                {"description", model.description ?? model.series },
                {"images", new BsonArray(images) },
                {"variants", new BsonArray()
                    {
                        new BsonDocument() { {"XS", ObjectId.GenerateNewId() } },
                        new BsonDocument() { {"S", ObjectId.GenerateNewId() } },
                        new BsonDocument() { {"M", ObjectId.GenerateNewId() } },
                        new BsonDocument() { {"L", ObjectId.GenerateNewId() } },
                        new BsonDocument() { {"XL", ObjectId.GenerateNewId() } }
                    }
                },
                {"display", model.display }
            };

            collection.InsertOneAsync(document).Wait();

            return createShirtFromBson(document);
        }

        public string uploadSeriesImage(string series, HttpPostedFileBase sliderImage)
        {
            var cloudinary = getCloudinary();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(series, sliderImage.InputStream),
                PublicId = series
            };

            return cloudinary.Upload(uploadParams).Uri.ToString();
        }

        public List<string> uploadImages(HttpPostedFileBase[] images, string title)
        {

            var cloudinary = getCloudinary();

            var urls = new List<string>();

            int numPhotos = Globals.Shirts.Exists(x => x.Title == title && x.Photos.Length > 0) ? 
                int.Parse(Globals.Shirts.FirstOrDefault(x => x.Title == title).Photos.Last().Split('_').Last().Replace(".png", "")) + 1 : 0;

            for (int i = numPhotos; i < images.Count() + numPhotos; i++)
            {
                var image = images[i - numPhotos];

                //var memoryStream = new MemoryStream();
                //image.InputStream.CopyTo(memoryStream);

                var safetitle = title.ToLower().Replace(' ', '_').Replace("'", "_").Replace('"', '_').Replace("/","_") + "_" + i;

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(safetitle, image.InputStream),
                    PublicId = safetitle
                };

                var upload = cloudinary.Upload(uploadParams);

                urls.Add(upload.Uri.ToString());
            }
            return urls;
        }

        public BsonDocument UpdateShirt(ShirtEditViewModel model)
        {
            var images = new BsonArray(model.sources);

            var filter = new BsonDocument("id", new ObjectId(model.Id));
            var update = Builders<BsonDocument>.Update
                .Set("title", model.title)
                .Set("type", model.series)
                .Set("handle", model.title.ToLower().Replace(' ', '-'))
                .Set("price", model.price)
                .Set("images", images)
                .Set("description", model.description ?? model.series);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After
            };
            return collection.FindOneAndUpdateAsync(filter, update, options).Result;
        }

        public BsonDocument UpdateShirt(string id, string field, string value)
        {
            var filter = new BsonDocument("id", new ObjectId(id));
            var update = Builders<BsonDocument>.Update
                .Set(field, value);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After
            };
            return collection.FindOneAndUpdateAsync(filter, update, options).Result;
        }

        public Shirt FindShirtById(string id)
        {
            return Globals.Shirts.FirstOrDefault(x => x.Id.ToString() == id);
        }

        public void deleteImage(string pub_id)
        {
            var cloudinary = getCloudinary();

            cloudinary.DeleteResources(new DelResParams()
            {
                PublicIds = new List<string>() { pub_id },
                Invalidate = true
            });
        }

        public Shirt deleteShirt(string id)
        {
            var shirt = FindShirtById(id);
            foreach(var photo in shirt.Photos)
            {
                deleteImage(photo.Split('/').Last().Replace(".png", ""));
            }

            var filter = new BsonDocument("id", new ObjectId(id));
            collection.DeleteOneAsync(filter).Wait();
            return FindShirtById(id);
        }

        public void UpdateShirt(Shirt shirt)
        {
            var filter = new BsonDocument("id", shirt.Id);
            var update = Builders<BsonDocument>.Update.Set("images", new BsonArray(shirt.Photos));
            collection.FindOneAndUpdateAsync(filter, update).Wait();
        }

        public Shirt createShirtFromBson(BsonDocument product)
        {
            var variants = new Dictionary<string, ObjectId>();

            foreach (var variant in new String[] { "XS", "S", "M", "L", "XL" })
            {
                variants.Add(variant, product["variants"].AsBsonArray.First(x => x.AsBsonDocument.Contains(variant))[variant].AsObjectId);
            }

            var shirt = new Shirt()
            {
                Id = product["id"].AsObjectId,
                Title = product["title"].AsString,
                Description = product["description"].AsString,
                Photos = product["images"].AsBsonArray.Select(x => x.ToString()).ToArray(),
                Handle = product["handle"].AsString,
                Price = product["price"].AsDouble,
                Stock = product["stock"].AsInt32,
                Type = product["type"].AsString,
                Display = bool.Parse(product["display"].AsString),
                Variants = variants
            };

            return shirt;
        }

        public string updateSeriesImage(string series, HttpPostedFileBase image)
        {
            deleteImage(series);
            return uploadSeriesImage(series, image);
        }

        public Shirt firstShirtOfType(string type)
        {
            return Globals.Shirts.FirstOrDefault(x => x.Type == type);
        }

        public Shirt firstDisplayedShirtOfType(string type)
        {
            return Globals.Shirts.FirstOrDefault(x => x.Type == type && x.Display);
        }
        
        private Cloudinary getCloudinary()
        {
            var account = new Account(Globals.CloudinaryName, Globals.CloudinaryAPIKey, Globals.CloudinarySecret);
            return new Cloudinary(account);
        }
    }
}