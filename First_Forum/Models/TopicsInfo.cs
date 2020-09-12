using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace First_Forum.Models
{
    public class TopicsInfo
    {
        public int Id { get; set; }
        public int Topic_id { get; set; }
        public int IdForum { get; set; }
        public string Subject { get; set; }
        public string CreatedBy { get; set; }
        public int Replies { get; set; }
    }
}