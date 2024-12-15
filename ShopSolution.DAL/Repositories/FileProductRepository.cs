using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    public class FileProductRepository : IProductRepository
    {
        private List<Product> _products = new();
        private int _nextProductId = 1;
        private readonly string _filePath;

        public FileProductRepository(string filePath)
        {
            _filePath = filePath;
            if (System.IO.File.Exists(_filePath))
            {
                // products.csv: productName;storeCode;quantity;price
                // Извлечём уникальные productName -> product
                var lines = System.IO.File.ReadAllLines(_filePath);
                var productNames = lines.Select(l=>l.Split(';')[0]).Distinct();
                foreach (var pn in productNames)
                {
                    _products.Add(new Product { Id=_nextProductId++, Name=pn });
                }
            }
        }

        public Task CreateAsync(string name)
        {
            if (_products.Any(x => x.Name == name))
                throw new Exception("Продукт уже существует");
            _products.Add(new Product { Id=_nextProductId++, Name=name });
            // Не записываем пока не будет в магазине
            return Task.CompletedTask;
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(x => x.Id == id));
        }

        public Task<Product?> GetByNameAsync(string name)
        {
            return Task.FromResult(_products.FirstOrDefault(x => x.Name == name));
        }
    }
}
