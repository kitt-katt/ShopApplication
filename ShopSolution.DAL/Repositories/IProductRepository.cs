using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByNameAsync(string name);
        Task<Product?> GetByIdAsync(int id);
        Task CreateAsync(string name);
    }

    public interface IExtendedProductRepository : IProductRepository
    {
        Task<List<Product>> GetAllAsync();
    }
}
