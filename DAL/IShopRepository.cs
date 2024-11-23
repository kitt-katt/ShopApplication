using ShopApplication.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApplication.DAL
{
    public interface IShopRepository
    {
        Task CreateShopAsync(Shop shop);
        Task<Shop> GetShopByCodeAsync(string code);
        Task<IEnumerable<Stock>> GetStocksByShopAsync(string shopCode);
        Task<IEnumerable<Stock>> GetStocksByProductAsync(string productName);
        Task AddStockAsync(Stock stock);
        Task UpdateStocksAsync(IEnumerable<Stock> stocks); // Для обновления остатков
        Task<IEnumerable<Shop>> GetAllShopsAsync();
    }
}
