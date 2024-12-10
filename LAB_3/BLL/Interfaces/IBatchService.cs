using LAB_3.DAL.Interfaces;
using LAB_3.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAB_3.BLL.Interfaces
{
    public interface IBatchService
    {
        event Action BatchesUpdated;
        Task<List<Batch>> GetAllBatchesAsync();
        Task<List<Batch>> GetBatchesByProductNameAsync(string productName);
        Task<List<Batch>> GetBatchesByStoreIdAsync(int storeId);
        Task AddBatchAsync(string productName, Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(string productName, int storeId);
        Task<(decimal TotalCost, bool IsPossible)> PurchaseBatchAsync(int storeId, string productName, int quantity);
    }
}
