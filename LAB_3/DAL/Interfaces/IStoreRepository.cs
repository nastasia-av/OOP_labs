using LAB_3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAB_3.DAL.Interfaces
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store?> GetStoreByIdAsync(int storeId);
        Task AddStoreAsync(Store store);
        Task UpdateStoreAsync(Store store);
        Task DeleteStoreAsync(int storeId);
    }
}
