using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThrowdownAttire.Models
{
    public partial class Shirt
    {
        public string Id { get; set; }

        public String Title { get; set; }

        public String Handle { get; set; }

        public String[] Photos { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public string Type { get; set; }

        public String Description { get; set; }

        public Dictionary<string, string> Sizes { get; set; } // Dictionary of each size and its ID. 
    }
}