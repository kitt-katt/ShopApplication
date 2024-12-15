using Microsoft.EntityFrameworkCore;
using ShopSolution.DAL.Context;
using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    public class RelationalStoreProductRepository : IStoreProductRepository
    {
        private readonly ShopContext _context;
        public RelationalStoreProductRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task<StoreProductInfo?> GetAsync(int storeId, int productId)
        {
            var sp = await _context.StoreProducts
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.StoreId == storeId && x.ProductId == productId);
            if (sp == null) return null;
            return new StoreProductInfo
            {
                StoreId = sp.StoreId,
                ProductName = sp.Product.Name,
                Price = sp.Price,
                Quantity = sp.Quantity
            };
        }

        public async Task<List<StoreProductPriceInfo>> GetStoresWithProductAsync(int productId)
        {
            return await _context.StoreProducts
                .Where(sp => sp.ProductId == productId && sp.Quantity > 0)
                .Select(sp => new StoreProductPriceInfo { StoreId = sp.StoreId, Price = sp.Price })
                .ToListAsync();
        }

        public async Task<List<StoreProductInfo>> GetProductsInStoreAsync(int storeId)
        {
            return await _context.StoreProducts
                .Where(sp => sp.StoreId == storeId && sp.Quantity > 0)
                .Include(x => x.Product)
                .Select(sp => new StoreProductInfo
                {
                    StoreId = sp.StoreId,
                    ProductName = sp.Product.Name,
                    Price = sp.Price,
                    Quantity = sp.Quantity
                })
                .ToListAsync();
        }

        public async Task UpdateQuantityAsync(int storeId, int productId, int newQuantity)
        {
            var sp = await _context.StoreProducts.FirstOrDefaultAsync(x => x.StoreId == storeId && x.ProductId == productId);
            if (sp == null) return;
            sp.Quantity = newQuantity;
            await _context.SaveChangesAsync();
        }

        public async Task UpsertAsync(int storeId, int productId, decimal price, int quantity)
        {
            var sp = await _context.StoreProducts.FirstOrDefaultAsync(x => x.StoreId == storeId && x.ProductId == productId);
            if (sp == null)
            {
                sp = new StoreProduct { StoreId = storeId, ProductId = productId, Price = price, Quantity = quantity };
                _context.StoreProducts.Add(sp);
            }
            else
            {
                sp.Price = price;
                sp.Quantity = quantity;
            }
            await _context.SaveChangesAsync();
        }
    }
}
