using ShopApplication.DAL;
using ShopApplication.Models;

namespace ShopApplication.BLL
{
    public class ShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public async Task CreateShopAsync(string code, string name, string address)
        {
            var shop = new Shop { Code = code, Name = name, Address = address };
            await _shopRepository.CreateShopAsync(shop);
        }

        public async Task CreateProductAsync(string name)
        {
            // Реализация для создания товара
        }

        public async Task StockProductAsync(string shopCode, string productName, int quantity, decimal price)
        {
            // Логика добавления товаров
        }

        public async Task<Shop> FindCheapestShopAsync(string productName)
        {
            // Логика поиска магазина с минимальной ценой
        }

        public async Task<IEnumerable<Stock>> GetAffordableItemsAsync(string shopCode, decimal budget)
        {
            // Логика поиска товаров, подходящих под бюджет
        }
    }
}
