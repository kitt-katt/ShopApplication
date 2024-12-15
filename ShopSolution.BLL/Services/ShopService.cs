using ShopSolution.BLL.DTO;
using ShopSolution.DAL.Repositories;

namespace ShopSolution.BLL.Services
{
    public class ShopService : IShopService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStoreProductRepository _storeProductRepository;

        public ShopService(IStoreRepository storeRepository, IProductRepository productRepository, IStoreProductRepository storeProductRepository)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
            _storeProductRepository = storeProductRepository;
        }

        public async Task CreateStoreAsync(string code, string name, string address)
        {
            var store = await _storeRepository.GetByCodeAsync(code);
            if (store != null) throw new Exception("Магазин с таким кодом уже существует.");
            await _storeRepository.CreateAsync(code, name, address);
        }

        public async Task CreateProductAsync(string productName)
        {
            var product = await _productRepository.GetByNameAsync(productName);
            if (product != null) throw new Exception("Товар с таким названием уже существует.");
            await _productRepository.CreateAsync(productName);
        }

        public async Task AddProductsToStoreAsync(string storeCode, List<PurchaseItemDTO> items)
        {
            var store = await _storeRepository.GetByCodeAsync(storeCode);
            if (store == null) throw new Exception("Магазин не найден.");
            foreach (var i in items)
            {
                var product = await _productRepository.GetByNameAsync(i.ProductName);
                if (product == null) throw new Exception($"Товар {i.ProductName} не найден.");
                await _storeProductRepository.UpsertAsync(store.Id, product.Id, i.Price, i.Quantity);
            }
        }

        public async Task<StoreDTO?> FindCheapestStoreForProductAsync(string productName)
        {
            var product = await _productRepository.GetByNameAsync(productName);
            if (product == null) return null;
            var prices = await _storeProductRepository.GetStoresWithProductAsync(product.Id);
            var cheapest = prices.OrderBy(x => x.Price).FirstOrDefault();
            if (cheapest == null) return null;
            var store = await _storeRepository.GetByIdAsync(cheapest.StoreId);
            if (store == null) return null;
            return new StoreDTO { Code = store.Code, Name = store.Name, Address = store.Address };
        }

        public async Task<List<PurchaseItemDTO>> GetProductsAffordableAsync(string storeCode, decimal budget)
        {
            var store = await _storeRepository.GetByCodeAsync(storeCode);
            if (store == null) throw new Exception("Магазин не найден.");
            var productsInStore = await _storeProductRepository.GetProductsInStoreAsync(store.Id);
            var result = new List<PurchaseItemDTO>();

            foreach (var p in productsInStore.OrderBy(x=>x.Price))
            {
                int maxQty = (int)Math.Floor(budget / p.Price);
                if (maxQty > p.Quantity) maxQty = p.Quantity;
                if (maxQty > 0)
                {
                    result.Add(new PurchaseItemDTO { ProductName = p.ProductName, Quantity = maxQty, Price = p.Price });
                    budget -= maxQty * p.Price;
                    if (budget <= 0) break;
                }
            }
            return result;
        }

        public async Task<decimal?> BuyProductsAsync(string storeCode, List<PurchaseItemDTO> items)
        {
            var store = await _storeRepository.GetByCodeAsync(storeCode);
            if (store == null) throw new Exception("Магазин не найден.");

            decimal totalCost = 0;
            foreach (var i in items)
            {
                var product = await _productRepository.GetByNameAsync(i.ProductName);
                if (product == null) return null;

                var storeProd = await _storeProductRepository.GetAsync(store.Id, product.Id);
                if (storeProd == null || storeProd.Quantity < i.Quantity) return null;
                totalCost += storeProd.Price * i.Quantity;
            }

            // Списываем
            foreach (var i in items)
            {
                var product = await _productRepository.GetByNameAsync(i.ProductName);
                var storeProd = await _storeProductRepository.GetAsync(store.Id, product.Id);
                await _storeProductRepository.UpdateQuantityAsync(store.Id, product.Id, storeProd!.Quantity - i.Quantity);
            }

            return totalCost;
        }

        public async Task<StoreDTO?> FindStoreForBulkPurchaseAsync(List<PurchaseItemDTO> items)
        {
            var stores = await _storeRepository.GetAllAsync();
            StoreDTO? bestStore = null;
            decimal bestPrice = decimal.MaxValue;

            foreach (var st in stores)
            {
                decimal sum = 0;
                bool canBuy = true;
                foreach (var it in items)
                {
                    var product = await _productRepository.GetByNameAsync(it.ProductName);
                    if (product == null)
                    {
                        canBuy = false;
                        break;
                    }
                    var sp = await _storeProductRepository.GetAsync(st.Id, product.Id);
                    if (sp == null || sp.Quantity < it.Quantity)
                    {
                        canBuy = false;
                        break;
                    }
                    sum += sp.Price * it.Quantity;
                }

                if (canBuy && sum < bestPrice)
                {
                    bestPrice = sum;
                    bestStore = new StoreDTO { Code = st.Code, Name = st.Name, Address = st.Address };
                }
            }

            return bestStore;
        }
    }
}
