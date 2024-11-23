using Microsoft.EntityFrameworkCore;
using ShopApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApplication.DAL
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateProductAsync(Product product)
        {
            // Добавляем новый продукт
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductByNameAsync(string name)
        {
            // Поиск продукта по названию
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            // Получение всех продуктов
            return await _context.Products.ToListAsync();
        }
    }
}
                                