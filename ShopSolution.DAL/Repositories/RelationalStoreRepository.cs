using Microsoft.EntityFrameworkCore;
using ShopSolution.DAL.Context;
using ShopSolution.DAL.Models;

namespace ShopSolution.DAL.Repositories
{
    public class RelationalStoreRepository : IStoreRepository
    {
        private readonly ShopContext _context;

        public RelationalStoreRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(string code, string name, string address)
        {
            var store = new Store { Code = code, Name = name, Address = address };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Store?> GetByCodeAsync(string code)
        {
            return await _context.Stores.FirstOrDefaultAsync(s => s.Code == code);
        }

        public async Task<Store?> GetByIdAsync(int id)
        {
            return await _context.Stores.FindAsync(id);
        }
    }
}
