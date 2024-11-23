using Microsoft.EntityFrameworkCore;
using ShopApplication.Models;

namespace ShopApplication.DAL
{
    public class ShopRepository : IShopRepository
    {
        private readonly ApplicationDbContext _context;

        public ShopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateShopAsync(Shop shop)
        {
            await _context.Shops.AddAsync(shop);
            await _context.SaveChangesAsync();
        }

        public async Task<Shop> GetShopByCodeAsync(string code)
        {
            return await _context.Shops.Include(s => s.Stocks).FirstOrDefaultAsync(s => s.Code == code);
        }
    }
}
