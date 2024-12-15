using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ShopSolution.DAL.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShopContext>
    {
        public ShopContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ShopContext>();
            builder.UseSqlite("Data Source=shop.db");
            return new ShopContext(builder.Options);
        }
    }
}
