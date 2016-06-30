using MongoDB.Driver;
using System.Linq;
using System.Web;
using ThrowdownAttire.Models;
using ThrowdownAttire.ViewModels;
using MongoDB.Bson;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ThrowdownAttire.App_Start;
using System.IO;

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

        public async Task Create(ShirtCreateViewModel model)
        {
            var images = uploadImages(model.images, model.title);

                await collection.InsertOneAsync(new BsonDocument()
                {
                    {"id", ObjectId.GenerateNewId() },
                    {"title", model.title },
                    {"handle", model.title.ToLower().Replace(' ', '-') },
                    {"type", model.series },
                    {"price", model.price },
                    {"stock", 4 },
                    {"description", model.description ?? model.series },
                    {"images", images },
                    {"variants", new BsonDocument()
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

        public BsonArray uploadImages(HttpPostedFileBase[] images, string title)
        {

            var account = new Account(Globals.CloudinaryName, Globals.CloudinaryAPIKey, Globals.CloudinarySecret);
            var cloudinary = new Cloudinary(account);

            var urls = new BsonArray();

            for (int i = 0; i < images.Count(); i++)
            {
                var image = images[i];

                //var memoryStream = new MemoryStream();
                //image.InputStream.CopyTo(memoryStream);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(title.ToLower().Replace(' ', '_') + "_" + i, image.InputStream),
                    PublicId = title.ToLower().Replace(' ', '_') + "_" + i
                };

                var upload = cloudinary.Upload(uploadParams);

                urls.Add(upload.Uri.ToString());
            }
            return urls;
        }
    }
}