using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    // Для простоты файловый репозиторий будет держать данные в памяти после чтения
    // Формат файла для магазинов: "code;name;address"
    public class FileStoreRepository : IStoreRepository
    {
        private List<Store> _stores = new();
        private int _nextId = 1;
        private readonly string _filePath;

        public FileStoreRepository(string filePath)
        {
            _filePath = filePath;
            if (System.IO.File.Exists(_filePath))
            {
                var lines = System.IO.File.ReadAllLines(_filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length>=3)
                    {
                        _stores.Add(new Store {
                            Id = _nextId++,
                            Code = parts[0],
                            Name = parts[1],
                            Address = parts[2]
                        });
                    }
                }
            }
        }

        public Task CreateAsync(string code, string name, string address)
        {
            var store = new Store { Id = _nextId++, Code=code, Name = name, Address=address};
            _stores.Add(store);
            SaveToFile();
            return Task.CompletedTask;
        }

        public Task<List<Store>> GetAllAsync()
        {
            return Task.FromResult(_stores.ToList());
        }

        public Task<Store?> GetByCodeAsync(string code)
        {
            return Task.FromResult(_stores.FirstOrDefault(x => x.Code == code));
        }

        public Task<Store?> GetByIdAsync(int id)
        {
            return Task.FromResult(_stores.FirstOrDefault(x => x.Id == id));
        }

        private void SaveToFile()
        {
            var lines = _stores.Select(s=>$"{s.Code};{s.Name};{s.Address}").ToArray();
            System.IO.File.WriteAllLines(_filePath, lines);
        }
    }
}
