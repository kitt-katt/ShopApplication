using Microsoft.EntityFrameworkCore;
using ShopSolution.DAL.Context;
using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    public class RelationalProductRepository : IProductRepository, IExtendedProductRepository
    {
        private readonly ShopContext _context;

        public RelationalProductRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(string name)
        {
            var product = new Product { Name = name };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
