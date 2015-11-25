using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThrowdownAttire.Models
{
    public partial class Shirt
    {
        public enum ShirtType
        {
            Drugs,
            Option2,
            Lifestyle,
            Fpp
        }

        public Guid Id { get; set; }

        public String Title { get; set; }

        public String Photo { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public ShirtType Type { get; set; }

        public String Description { get; set; }
    }
}