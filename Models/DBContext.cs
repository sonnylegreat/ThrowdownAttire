using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace ThrowdownAttire.Models
{
    public class DBContext
    {
        public IMongoDatabase GetDatabase()
        {
            var client = new MongoClient(ConfigurationManager.AppSettings["ConnectionString"]);
            return client.GetDatabase(ConfigurationManager.AppSettings["DBName"]);
        }
    }
}