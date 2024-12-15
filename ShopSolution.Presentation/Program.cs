using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ShopSolution.BLL.Services;
using ShopSolution.DAL.Context;
using ShopSolution.DAL.Repositories;
using ShopSolution.BLL.DTO;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;
                services.AddDbContext<ShopContext>(options =>
                {
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                });

                services.AddScoped<IStoreRepository, RelationalStoreRepository>();
                services.AddScoped<IProductRepository, RelationalProductRepository>();
                services.AddScoped<IStoreProductRepository, RelationalStoreProductRepository>();
                services.AddScoped<IShopService, ShopService>();
            });

        var host = builder.Build();

        // Применяем миграции
        using (var scope = host.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ShopContext>();
            await context.Database.MigrateAsync();
        }

        var service = host.Services.GetRequiredService<IShopService>();

        while(true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1: Создать магазин");
            Console.WriteLine("2: Создать товар");
            Console.WriteLine("3: Завезти партию товаров в магазин");
            Console.WriteLine("4: Найти магазин, где товар самый дешевый");
            Console.WriteLine("5: Узнать какие товары можно купить на сумму");
            Console.WriteLine("6: Купить партию товаров в магазине");
            Console.WriteLine("7: Найти магазин для партии товаров с минимальной суммой");
            Console.WriteLine("8: Вывести все товары");
            Console.WriteLine("9: Вывести все магазины и их товары");
            Console.WriteLine("0: Выход");
            Console.Write("Ваш выбор: ");

            var choice = Console.ReadLine();
            if (choice == "0") break;

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите код магазина: ");
                        var code = Console.ReadLine() ?? "";
                        Console.Write("Введите название магазина: ");
                        var name = Console.ReadLine() ?? "";
                        Console.Write("Введите адрес магазина: ");
                        var address = Console.ReadLine() ?? "";
                        await service.CreateStoreAsync(code, name, address);
                        Console.WriteLine("Магазин создан.");
                        break;
                    case "2":
                        Console.Write("Введите название товара: ");
                        var pname = Console.ReadLine() ?? "";
                        await service.CreateProductAsync(pname);
                        Console.WriteLine("Товар создан.");
                        break;
                    case "3":
                        Console.Write("Введите код магазина: ");
                        var scode = Console.ReadLine() ?? "";
                        Console.WriteLine("Введите партии товаров (формат: Название;Кол-во;Цена), пустая строка для завершения:");
                        var items = new List<PurchaseItemDTO>();
                        while(true)
                        {
                            var line = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(line)) break;
                            var parts = line.Split(';');
                            if (parts.Length < 3) {Console.WriteLine("Неверный формат"); continue;}
                            var iname = parts[0];
                            var iqty = int.Parse(parts[1]);
                            var iprice = decimal.Parse(parts[2]);
                            items.Add(new PurchaseItemDTO{ProductName=iname, Quantity=iqty, Price=iprice});
                        }
                        await service.AddProductsToStoreAsync(scode, items);
                        Console.WriteLine("Партия товаров завезена.");
                        break;
                    case "4":
                        Console.Write("Введите название товара: ");
                        var cprod = Console.ReadLine() ?? "";
                        var cheapestStore = await service.FindCheapestStoreForProductAsync(cprod);
                        if (cheapestStore == null)
                            Console.WriteLine("Не найден магазин с таким товаром.");
                        else
                            Console.WriteLine($"Самый дешевый магазин: {cheapestStore.Code} ({cheapestStore.Name}, {cheapestStore.Address})");
                        break;
                    case "5":
                        Console.Write("Введите код магазина: ");
                        var stcode = Console.ReadLine() ?? "";
                        Console.Write("Введите сумму: ");
                        var budget = decimal.Parse(Console.ReadLine() ?? "0");
                        var affordable = await service.GetProductsAffordableAsync(stcode, budget);
                        Console.WriteLine("Вы можете купить:");
                        foreach (var it in affordable)
                        {
                            Console.WriteLine($"{it.ProductName} x {it.Quantity} по цене {it.Price} руб.");
                        }
                        break;
                    case "6":
                        Console.Write("Введите код магазина: ");
                        var buycode = Console.ReadLine() ?? "";
                        Console.WriteLine("Введите покупаемые товары (формат: Название;Кол-во), пустая строка для завершения:");
                        var buyItems = new List<PurchaseItemDTO>();
                        while(true)
                        {
                            var line = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(line)) break;
                            var parts = line.Split(';');
                            if (parts.Length < 2) {Console.WriteLine("Неверный формат"); continue;}
                            var iname = parts[0];
                            var iqty = int.Parse(parts[1]);
                            buyItems.Add(new PurchaseItemDTO{ProductName=iname, Quantity=iqty});
                        }
                        var totalCost = await service.BuyProductsAsync(buycode, buyItems);
                        if (totalCost.HasValue)
                            Console.WriteLine($"Покупка успешно совершена. Общая стоимость: {totalCost} руб.");
                        else
                            Console.WriteLine("Покупка невозможна (товара не хватает или нет).");
                        break;
                    case "7":
                        Console.WriteLine("Введите товары для партии (формат: Название;Кол-во), пустая строка для завершения:");
                        var bulkItems = new List<PurchaseItemDTO>();
                        while(true)
                        {
                            var line = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(line)) break;
                            var parts = line.Split(';');
                            if (parts.Length < 2) {Console.WriteLine("Неверный формат"); continue;}
                            var iname = parts[0];
                            var iqty = int.Parse(parts[1]);
                            bulkItems.Add(new PurchaseItemDTO{ProductName=iname, Quantity=iqty});
                        }
                        var bestStore = await service.FindStoreForBulkPurchaseAsync(bulkItems);
                        if (bestStore == null)
                            Console.WriteLine("Нет подходящего магазина или недостаточно товара.");
                        else
                            Console.WriteLine($"Лучший магазин: {bestStore.Code} ({bestStore.Name}, {bestStore.Address})");
                        break;
                    case "8":
                        var allProducts = await service.GetAllProductsAsync();
                        Console.WriteLine("Список всех товаров:");
                        foreach (var p in allProducts)
                        {
                            Console.WriteLine($"- {p.Name}");
                        }
                        break;
                    case "9":
                        var storesWithProducts = await service.GetAllStoresWithProductsAsync();
                        Console.WriteLine("Список всех магазинов и их товаров:");
                        foreach (var kvp in storesWithProducts)
                        {
                            Console.WriteLine($"Магазин {kvp.Key.Code} ({kvp.Key.Name}, {kvp.Key.Address}):");
                            if (kvp.Value.Count == 0)
                            {
                                Console.WriteLine("  Нет товаров");
                            }
                            else
                            {
                                foreach (var it in kvp.Value)
                                {
                                    Console.WriteLine($"  {it.ProductName} x {it.Quantity}, цена: {it.Price}");
                                }
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        Console.WriteLine("Выход из программы.");
    }
}
