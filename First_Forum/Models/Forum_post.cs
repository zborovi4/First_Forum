namespace First_Forum.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Forum_post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(250)]
        public string Author { get; set; }

        [StringLength(500)]
        public string Topic { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime Date { get; set; }

        public int Post_id { get; set; }

        public int Topic_id { get; set; }

        public int Forum_id { get; set; }

        public virtual Forum Forum { get; set; }
    }
}
