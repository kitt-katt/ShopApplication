using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    public interface IStoreRepository
    {
        Task<Store?> GetByCodeAsync(string code);
        Task<Store?> GetByIdAsync(int id);
        Task<List<Store>> GetAllAsync();
        Task CreateAsync(string code, string name, string address);
    }
}
