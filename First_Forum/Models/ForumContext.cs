namespace First_Forum.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ForumContext : DbContext
    {
        public ForumContext()
            : base("name=ForumContext")
        {
        }

        public virtual DbSet<Forum> Fora { get; set; }
        public virtual DbSet<Forum_post> Forum_post { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
