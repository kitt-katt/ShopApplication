using ShopApplication.DAL;
using ShopApplication.Models;

namespace ShopApplication.BLL
{
    public class ShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IProductRepository _productRepository;

        public ShopService(IShopRepository shopRepository, IProductRepository productRepository)
        {
            _shopRepository = shopRepository;
            _productRepository = productRepository;
        }

        public async Task CreateShopAsync(string code, string name, string address)
        {
            var shop = new Shop { Code = code, Name = name, Address = address };
            await _shopRepository.CreateShopAsync(shop);
        }

        public async Task CreateProductAsync(string name)
        {
            var product = new Product { Name = name };
            await _productRepository.CreateProductAsync(product);
        }

        public async Task StockProductAsync(string shopCode, string productName, int quantity, decimal price)
        {
            var stock = new Stock
            {
                ShopCode = shopCode,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };
            await _shopRepository.AddStockAsync(stock);
        }

        public async Task<Shop> FindCheapestShopAsync(string productName)
        {
            var stocks = await _shopRepository.GetStocksByProductAsync(productName);
            var cheapestStock = stocks.OrderBy(s => s.Price).FirstOrDefault();
            return cheapestStock != null ? await _shopRepository.GetShopByCodeAsync(cheapestStock.ShopCode) : null;
        }

        public async Task<IEnumerable<Stock>> GetAffordableItemsAsync(string shopCode, decimal budget)
        {
            var stocks = await _shopRepository.GetStocksByShopAsync(shopCode);
            return stocks.Where(s => s.Price <= budget).ToList();
        }

        public async Task<decimal?> BuyProductsAsync(string shopCode, Dictionary<string, int> products)
        {
            var stocks = await _shopRepository.GetStocksByShopAsync(shopCode);

            decimal totalCost = 0;
            foreach (var (productName, quantity) in products)
            {
                var stock = stocks.FirstOrDefault(s => s.ProductName == productName);
                if (stock == null || stock.Quantity < quantity)
                    return null; // Недостаточно товаров или товара нет в наличии.

                totalCost += stock.Price * quantity;
                stock.Quantity -= quantity;
            }

            await _shopRepository.UpdateStocksAsync(stocks);
            return totalCost;
        }

        public async Task<Shop> FindCheapestShopForProductsAsync(Dictionary<string, int> products)
        {
            var allShops = await _shopRepository.GetAllShopsAsync();
            Shop cheapestShop = null;
            decimal? minCost = null;

            foreach (var shop in allShops)
            {
                var stocks = await _shopRepository.GetStocksByShopAsync(shop.Code);
                decimal totalCost = 0;

                foreach (var (productName, quantity) in products)
                {
                    var stock = stocks.FirstOrDefault(s => s.ProductName == productName);
                    if (stock == null || stock.Quantity < quantity)
                    {
                        totalCost = -1; // Нельзя купить все товары в этом магазине.
                        break;
                    }
                    totalCost += stock.Price * quantity;
                }

                if (totalCost != -1 && (minCost == null || totalCost < minCost))
                {
                    minCost = totalCost;
                    cheapestShop = shop;
                }
            }

            return cheapestShop;
        }
    }
}
