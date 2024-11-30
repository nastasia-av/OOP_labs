using LAB_3.BLL.Interfaces;
using LAB_3.DAL.Interfaces;
using LAB_3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_3.BLL.Services
{
    public class BatchService : IBatchService
    {
        private readonly IProductRepository _productRepository;

        public event Action BatchesUpdated;

        public BatchService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Batch>> GetAllBatchesAsync()
        {
            return (await _productRepository.GetAllBatchesAsync()).ToList();
        }

        public async Task<List<Batch>> GetBatchesByProductNameAsync(string productName)
        {
            return (await _productRepository.GetBatchesByProductNameAsync(productName)).ToList();
        }

        public async Task<List<Batch>> GetBatchesByStoreIdAsync(int storeId)
        {
            return (await _productRepository.GetBatchesByStoreIdAsync(storeId)).ToList();
        }

        public async Task AddBatchAsync(string productName, Batch batch)
        {
            await _productRepository.AddBatchToProductAsync(productName, batch);
            BatchesUpdated?.Invoke();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            await _productRepository.UpdateBatchAsync(batch);
            BatchesUpdated?.Invoke();
        }

        public async Task DeleteBatchAsync(string productName, int storeId)
        {
            await _productRepository.DeleteBatchAsync(productName, storeId);
            BatchesUpdated?.Invoke();
        }

        public async Task<(decimal TotalCost, bool IsPossible)> PurchaseBatchAsync(int storeId, string productName, int quantity)
        {
            var batches = await _productRepository.GetBatchesByStoreIdAsync(storeId);
            var batch = batches.FirstOrDefault(b => b.ProductName == productName && b.Quantity >= quantity);
            decimal totalCost = 0;

            if (batch == null)
            {
                return (totalCost, false);
            }
            else {
                totalCost = batch.Price * quantity;
                batch.Quantity -= quantity;
                await _productRepository.UpdateBatchAsync(batch);
                BatchesUpdated?.Invoke();
            }
            
            return (totalCost, true);
        }
    }
}
