using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThrowdownAttire.Models;

namespace ThrowdownAttire.ViewModels
{
    public class ShirtCreateViewModel
    {
        public string title { get; set; }
        public string series { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public CloudinaryDotNet.Cloudinary cloudinary { get; set; }
        public HttpPostedFileBase[] images { get; set; }
    }

    public class ShirtCreateViewViewModel
    {
        public List<Shirt> shirts;
        public ShirtCreateViewModel shirtCreateModel;
    }

    public class ShirtEditViewModel
    {
        public string title { get; set; }
        public string series { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public HttpPostedFileBase[] images { get; set; }
    }
}