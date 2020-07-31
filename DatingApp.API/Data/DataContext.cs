using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        
        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Composite primary key of two columns
            builder.Entity<Like>()
                .HasKey(k => new {k.LikerId, k.LikeeId});

            // Defining many to many relationship 
            builder.Entity<Like>()    
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                // Prevents cascading deletion
                .OnDelete(DeleteBehavior.Restrict);

              builder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                // Prevents cascading deletion
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}