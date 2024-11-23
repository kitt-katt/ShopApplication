using Microsoft.EntityFrameworkCore;
using ShopApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.Shops
                .Include(s => s.Stocks)
                .FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<IEnumerable<Stock>> GetStocksByShopAsync(string shopCode)
        {
            return await _context.Stocks
                .Where(s => s.ShopCode == shopCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<Stock>> GetStocksByProductAsync(string productName)
        {
            return await _context.Stocks
                .Where(s => s.ProductName == productName)
                .ToListAsync();
        }

        public async Task AddStockAsync(Stock stock)
        {
            // Проверяем, существует ли уже запись для этого товара в магазине
            var existingStock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.ShopCode == stock.ShopCode && s.ProductName == stock.ProductName);

            if (existingStock == null)
            {
                // Если записи нет, добавляем новую
                await _context.Stocks.AddAsync(stock);
            }
            else
            {
                // Если запись есть, обновляем количество и цену
                existingStock.Quantity += stock.Quantity;
                existingStock.Price = stock.Price;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateStocksAsync(IEnumerable<Stock> stocks)
        {
            _context.Stocks.UpdateRange(stocks);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Shop>> GetAllShopsAsync()
        {
            return await _context.Shops.ToListAsync();
        }
    }
}
