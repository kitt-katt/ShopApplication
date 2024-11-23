using Microsoft.EntityFrameworkCore;
using ShopApplication.Models;

namespace ShopApplication.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shopapp.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shop>().HasKey(s => s.Code);
            modelBuilder.Entity<Product>().HasKey(p => p.Name);
            modelBuilder.Entity<Stock>().HasKey(s => s.Id);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Shop)
                .WithMany(s => s.Stocks)
                .HasForeignKey(s => s.ShopCode);

            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.ProductName);
        }
    }
}