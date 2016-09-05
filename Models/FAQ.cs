using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThrowdownAttire.Models
{
    public class FAQ
    {
        public string Category { get; set; }

        public List<string> Questions { get; set; }

        public List<string> Answers { get; set; }
    }
}