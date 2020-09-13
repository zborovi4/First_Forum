using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace First_Forum.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Author { get; set; }

        public string Topic { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public int Post_id { get; set; }

        public int Topic_id { get; set; }

        public int Forum_id { get; set; }
    }
}