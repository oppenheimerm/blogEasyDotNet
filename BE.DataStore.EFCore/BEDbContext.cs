
using BE.Core;
using Microsoft.EntityFrameworkCore;

namespace BE.DataStore.EFCore
{
    public class BEDbContext : DbContext
    {
        public BEDbContext(DbContextOptions<BEDbContext> options)
            : base(options)
        { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Error Fix: The entity type ‘IdentityUserLogin‘ requires a primary
            //  key to be defined. If you intended to use a keyless entity
            //  type call ‘HasNoKey()’.
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<PostTag>().ToTable("PostTag");
        }
    }
}
