using LAB_3.DAL.Interfaces;
using LAB_3.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_3.DAL.Repositories
{
    public class FileStoreRepository : IStoreRepository
    {
        private readonly string _filePath;

        public FileStoreRepository(string filePath)
        {
            _filePath = Path.Combine(Environment.CurrentDirectory, filePath); 
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            if (!File.Exists(_filePath)) return new List<Store>();

            var lines = await File.ReadAllLinesAsync(_filePath);
            return lines.Select(ParseStoreFromCsv).Where(store => store != null).Cast<Store>();
        }

        public async Task<Store?> GetStoreByIdAsync(int storeId)
        {
            var stores = await GetAllStoresAsync();
            return stores.FirstOrDefault(store => store.StoreId == storeId);
        }

        public async Task AddStoreAsync(Store store)
        {
            var stores = await GetAllStoresAsync();

            int newStoreId = stores.Any() ? stores.Max(s => s.StoreId) + 1 : 1;
            store.StoreId = newStoreId;

            var line = $"{store.StoreId},{store.Name},{store.Address}";
            await File.AppendAllTextAsync(_filePath, line + "\n", Encoding.UTF8);
        }

        public async Task UpdateStoreAsync(Store store)
        {
            var stores = (await GetAllStoresAsync()).ToList();
            var existingStore = stores.FirstOrDefault(s => s.StoreId == store.StoreId);
            if (existingStore != null)
            {
                existingStore.Name = store.Name;
                existingStore.Address = store.Address;

                await SaveAllStoresAsync(stores);
            }
        }

        public async Task DeleteStoreAsync(int storeId)
        {
            var stores = (await GetAllStoresAsync()).ToList();
            stores.RemoveAll(s => s.StoreId == storeId);
            await SaveAllStoresAsync(stores);
        }

        private async Task SaveAllStoresAsync(IEnumerable<Store> stores)
        {
            var lines = stores.Select(store => $"{store.StoreId},{store.Name},{store.Address}");
            await File.WriteAllLinesAsync(_filePath, lines, Encoding.UTF8);
        }

        private static Store? ParseStoreFromCsv(string line)
        {
            var parts = line.Split(',');
            if (parts.Length < 3) return null;

            if (int.TryParse(parts[0], out int storeId))
            {
                return new Store
                {
                    StoreId = storeId,
                    Name = parts[1],
                    Address = parts[2]
                };
            }
            return null;
        }
    }
}
