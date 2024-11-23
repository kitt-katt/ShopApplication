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
            // Проверка существования магазина
            var shop = await _shopRepository.GetShopByCodeAsync(shopCode);
            if (shop == null)
            {
                Console.WriteLine("Магазин с указанным кодом не найден.");
                return;
            }

            // Проверка существования товара
            var product = await _productRepository.GetProductByNameAsync(productName);
            if (product == null)
            {
                Console.WriteLine("Товар с указанным названием не найден.");
                return;
            }

            // Добавление товара в магазин
            var stock = new Stock
            {
                ShopCode = shopCode,
                ProductName = productName,
                Quantity = quantity,
                Price = price
            };

            await _shopRepository.AddStockAsync(stock);
            Console.WriteLine("Товар успешно добавлен в магазин.");
        }

        public async Task<Shop> FindCheapestShopAsync(string productName)
        {
            var stocks = await _shopRepository.GetStocksByProductAsync(productName);
            if (!stocks.Any())
            {
                Console.WriteLine("Товар не найден в наличии ни в одном магазине.");
                return null;
            }

            var cheapestStock = stocks.OrderBy(s => s.Price).FirstOrDefault();
            return cheapestStock != null ? await _shopRepository.GetShopByCodeAsync(cheapestStock.ShopCode) : null;
        }

        public async Task<IEnumerable<Stock>> GetAffordableItemsAsync(string shopCode, decimal budget)
        {
            var stocks = await _shopRepository.GetStocksByShopAsync(shopCode);
            if (!stocks.Any())
            {
                Console.WriteLine("В магазине нет товаров.");
                return Enumerable.Empty<Stock>();
            }

            return stocks
                .Where(s => s.Price <= budget)
                .OrderBy(s => s.Price)
                .ToList();
        }

        public async Task<decimal?> BuyProductsAsync(string shopCode, Dictionary<string, int> products)
        {
            var stocks = await _shopRepository.GetStocksByShopAsync(shopCode);
            if (!stocks.Any())
            {
                Console.WriteLine("В магазине нет товаров.");
                return null;
            }

            decimal totalCost = 0;

            foreach (var (productName, quantity) in products)
            {
                var stock = stocks.FirstOrDefault(s => s.ProductName == productName);
                if (stock == null || stock.Quantity < quantity)
                {
                    Console.WriteLine($"Недостаточно товара {productName} в магазине.");
                    return null;
                }

                totalCost += stock.Price * quantity;
                stock.Quantity -= quantity; // Обновляем количество
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

        public async Task<IEnumerable<Shop>> GetAllShopsAsync()
        {
            return await _shopRepository.GetAllShopsAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }
    }
}
