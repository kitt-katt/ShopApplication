using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopApplication.BLL;
using ShopApplication.DAL;


namespace ShopApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var shopService = services.GetRequiredService<ShopService>();

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Создать магазин");
                Console.WriteLine("2. Создать товар");
                Console.WriteLine("3. Завезти товар в магазин");
                Console.WriteLine("4. Найти магазин с самым дешевым товаром");
                Console.WriteLine("5. Узнать, что можно купить на сумму");
                Console.WriteLine("6. Купить товары в магазине");
                Console.WriteLine("7. Найти магазин с минимальной стоимостью партии");
                Console.WriteLine("8. Показать все магазины");
                Console.WriteLine("9. Показать все товары");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await CreateShop(shopService);
                        break;
                    case "2":
                        await CreateProduct(shopService);
                        break;
                    case "3":
                        await StockProduct(shopService);
                        break;
                    case "4":
                        await FindCheapestShop(shopService);
                        break;
                    case "5":
                        await GetAffordableItems(shopService);
                        break;
                    case "6":
                        await BuyProducts(shopService);
                        break;
                    case "7":
                        await FindCheapestShopForProducts(shopService);
                        break;
                    case "8":
                        await ShowAllShops(shopService);
                        break;
                    case "9":
                        await ShowAllProducts(shopService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>();
                    services.AddScoped<IShopRepository, ShopRepository>();
                    services.AddScoped<IProductRepository, ProductRepository>();
                    services.AddScoped<ShopService>();
                });

        private static async Task CreateShop(ShopService shopService)
        {
            Console.Write("Введите код магазина: ");
            var code = Console.ReadLine();
            Console.Write("Введите название магазина: ");
            var name = Console.ReadLine();
            Console.Write("Введите адрес магазина: ");
            var address = Console.ReadLine();

            await shopService.CreateShopAsync(code, name, address);
            Console.WriteLine("Магазин создан.");
        }

        private static async Task CreateProduct(ShopService shopService)
        {
            Console.Write("Введите название товара: ");
            var name = Console.ReadLine();

            await shopService.CreateProductAsync(name);
            Console.WriteLine("Товар создан.");
        }

        private static async Task StockProduct(ShopService shopService)
        {
            Console.Write("Введите код магазина: ");
            var shopCode = Console.ReadLine();
            Console.Write("Введите название товара: ");
            var productName = Console.ReadLine();
            Console.Write("Введите количество товара: ");
            var quantity = int.Parse(Console.ReadLine());
            Console.Write("Введите цену товара: ");
            var price = decimal.Parse(Console.ReadLine());

            await shopService.StockProductAsync(shopCode, productName, quantity, price);
            Console.WriteLine("Товар завезен.");
        }

        private static async Task FindCheapestShop(ShopService shopService)
        {
            Console.Write("Введите название товара: ");
            var productName = Console.ReadLine();

            var shop = await shopService.FindCheapestShopAsync(productName);
            Console.WriteLine(shop != null
                ? $"Самый дешевый магазин: {shop.Name}"
                : "Товар не найден.");
        }

        private static async Task GetAffordableItems(ShopService shopService)
        {
            Console.Write("Введите код магазина: ");
            var shopCode = Console.ReadLine();
            Console.Write("Введите сумму: ");
            var budget = decimal.Parse(Console.ReadLine());

            var items = await shopService.GetAffordableItemsAsync(shopCode, budget);
            Console.WriteLine("Доступные товары:");
            foreach (var item in items)
                Console.WriteLine($"{item.ProductName} - {item.Quantity} шт. за {item.Price} руб.");
        }

        private static async Task BuyProducts(ShopService shopService)
        {
            Console.Write("Введите код магазина: ");
            var shopCode = Console.ReadLine();
            Console.Write("Введите товары (название:количество, через запятую): ");
            var input = Console.ReadLine();

            var products = input.Split(',')
                .Select(p => p.Split(':'))
                .ToDictionary(p => p[0], p => int.Parse(p[1]));

            var cost = await shopService.BuyProductsAsync(shopCode, products);
            Console.WriteLine(cost != null
                ? $"Общая стоимость покупки: {cost} руб."
                : "Покупка невозможна. Недостаточно товаров.");
        }

        private static async Task FindCheapestShopForProducts(ShopService shopService)
        {
            Console.Write("Введите товары (название:количество, через запятую): ");
            var input = Console.ReadLine();

            var products = input.Split(',')
                .Select(p => p.Split(':'))
                .ToDictionary(p => p[0], p => int.Parse(p[1]));

            var shop = await shopService.FindCheapestShopForProductsAsync(products);
            Console.WriteLine(shop != null
                ? $"Самый дешевый магазин: {shop.Name}"
                : "Не найдено магазина для указанной партии.");
        }
        private static async Task ShowAllShops(ShopService shopService)
        {
            var shops = await shopService.GetAllShopsAsync();
            Console.WriteLine("\nСписок магазинов:");
            foreach (var shop in shops)
            {
                Console.WriteLine($"Код: {shop.Code}, Название: {shop.Name}, Адрес: {shop.Address}");
            }
        }

        private static async Task ShowAllProducts(ShopService shopService)
        {
            var products = await shopService.GetAllProductsAsync();
            Console.WriteLine("\nСписок товаров:");
            foreach (var product in products)
            {
                Console.WriteLine($"Название: {product.Name}");
            }
        }
    }
}
