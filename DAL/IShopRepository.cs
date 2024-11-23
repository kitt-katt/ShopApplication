using ShopApplication.Models;

namespace ShopApplication.DAL
{
    public interface IShopRepository
    {
        Task CreateShopAsync(Shop shop);
        Task<Shop> GetShopByCodeAsync(string code);
        // Другие методы CRUD
    }
}
