using Microsoft.EntityFrameworkCore;
using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Context
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

        public DbSet<Store> Stores { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<StoreProduct> StoreProducts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoreProduct>().HasKey(sp => new { sp.StoreId, sp.ProductId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
