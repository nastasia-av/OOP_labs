using LAB_3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAB_3.BLL.Interfaces
{
    public interface IStoreService
    {
        event Action StoresUpdated;
        Task<List<Store>> GetAllStoresAsync();
        Task<Store?> GetStoreByIdAsync(int storeId);
        Task AddStoreAsync(Store store);
        Task UpdateStoreAsync(Store store);
        Task DeleteStoreAsync(int storeId);
        Task<(Store? Store, decimal cost)?> FindCheapestStoreAsync(string productName);
        Task<(Store? Store, decimal TotalCost)?> FindStoreWithCheapestBatchAsync(string productName, int quantity);
        Task<List<(string ProductName, int Quantity, decimal TotalPrice)>> GetAffordableProductsAsync(int storeId, decimal budget);
    }
}
