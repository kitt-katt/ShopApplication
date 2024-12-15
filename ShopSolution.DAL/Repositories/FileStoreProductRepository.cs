using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    // Формат products.csv: productName;storeCode;quantity;price
    public class FileStoreProductRepository : IStoreProductRepository
    {
        private readonly string _filePath;
        private List<(string productName, string storeCode, int quantity, decimal price)> _entries = new();

        public FileStoreProductRepository(string filePath)
        {
            _filePath = filePath;
            if (System.IO.File.Exists(_filePath))
            {
                var lines = System.IO.File.ReadAllLines(_filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length>=4)
                    {
                        var productName = parts[0];
                        var storeCode = parts[1];
                        var quantity = int.Parse(parts[2]);
                        var price = decimal.Parse(parts[3]);
                        _entries.Add((productName, storeCode, quantity, price));
                    }
                }
            }
        }
        public async Task UpsertAsync(int storeId, int productId, decimal price, int quantity)
        {
            throw new NotImplementedException("File repository does not support ID-based operations directly.");
        }

        public async Task UpdateQuantityAsync(int storeId, int productId, int newQuantity)
        {
            throw new NotImplementedException("File repository does not support ID-based operations directly.");
        }

        public async Task<List<StoreProductPriceInfo>> GetStoresWithProductAsync(int productId)
        {
            throw new NotImplementedException("File repository does not support ID-based operations directly.");
        }

        public async Task<List<StoreProductInfo>> GetProductsInStoreAsync(int storeId)
        {
            throw new NotImplementedException("File repository does not support ID-based operations directly.");
        }

        public async Task<StoreProductInfo?> GetAsync(int storeId, int productId)
        {
            throw new NotImplementedException("File repository does not support ID-based operations directly.");
        }
    }
}
