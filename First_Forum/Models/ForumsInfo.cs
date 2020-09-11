using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace First_Forum.Models
{
    public class ForumsInfo
    {
        public int Id { get; set; }
        public string Board { get; set; }
        public int Threads { get; set; }
        public int Posts { get; set; }
    }
}