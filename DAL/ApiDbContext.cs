using System;
using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApiDbContext : IdentityDbContext
    {
        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<PasswordReset> PasswordReset { get; set; }

        public virtual DbSet<Category> Category { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<OrderDetails> OrderDetails { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasOne(b => b.Category)
            .WithMany(a => a.Products)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
