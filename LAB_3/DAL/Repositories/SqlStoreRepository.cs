using LAB_3.DAL.Interfaces;
using LAB_3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_3.DAL.Repositories
{
    public class SqlStoreRepository : IStoreRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlStoreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<Store?> GetStoreByIdAsync(int storeId)
        {
            return await _context.Stores.FirstOrDefaultAsync(s => s.StoreId == storeId);
        }

        public async Task AddStoreAsync(Store store)
        {
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStoreAsync(Store store)
        {
            var existingStore = await _context.Stores.FirstOrDefaultAsync(s => s.StoreId == store.StoreId);
            if (existingStore != null)
            {
                existingStore.Name = store.Name;
                existingStore.Address = store.Address;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteStoreAsync(int storeId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.StoreId == storeId);
            if (store != null)
            {
                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
            }
        }
    }
}
