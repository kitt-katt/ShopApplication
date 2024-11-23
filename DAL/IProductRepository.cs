using ShopApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApplication.DAL
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);
        Task<Product> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
