using LAB_3.BLL.Interfaces;
using LAB_3.DAL.Interfaces;
using LAB_3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_3.BLL.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;

        public event Action StoresUpdated;

        public StoreService(IStoreRepository storeRepository, IProductRepository productRepository)
        {
            _storeRepository = storeRepository;
            _productRepository = productRepository;
        }

        public async Task<List<Store>> GetAllStoresAsync()
        {
            var stores = await _storeRepository.GetAllStoresAsync();
            return stores?.Where(store => store != null).ToList() ?? new List<Store>();
        }

        public async Task<Store?> GetStoreByIdAsync(int storeId)
        {
            return await _storeRepository.GetStoreByIdAsync(storeId);
        }

        public async Task AddStoreAsync(Store store)
        {
            await _storeRepository.AddStoreAsync(store);
            StoresUpdated?.Invoke();
        }

        public async Task UpdateStoreAsync(Store store)
        {
            await _storeRepository.UpdateStoreAsync(store);
            StoresUpdated?.Invoke();
        }

        public async Task DeleteStoreAsync(int storeId)
        {
            var batches = await _productRepository.GetBatchesByStoreIdAsync(storeId);
            foreach (var batch in batches)
            {
                await _productRepository.DeleteBatchAsync(batch.ProductName, batch.StoreId);
            }
            await _storeRepository.DeleteStoreAsync(storeId);
            StoresUpdated?.Invoke();
        }

        public async Task<(Store? Store, decimal cost)?> FindCheapestStoreAsync(string productName)
        {
            var batches = await _productRepository.GetBatchesByProductNameAsync(productName);
            var cheapestBatch = batches.OrderBy(b => b.Price).FirstOrDefault();

            return cheapestBatch != null
                ? (await _storeRepository.GetStoreByIdAsync(cheapestBatch.StoreId), cheapestBatch.Price)
                : null;
        }


        public async Task<(Store? Store, decimal TotalCost)?> FindStoreWithCheapestBatchAsync(string productName, int quantity)
        {
            var batches = await _productRepository.GetBatchesByProductNameAsync(productName);
            var cheapestBatch = batches
            .Where(b => b.Quantity >= quantity)
            .OrderBy(b => b.Price)
            .FirstOrDefault();

            return cheapestBatch != null
                ? (await _storeRepository.GetStoreByIdAsync(cheapestBatch.StoreId), 
                cheapestBatch.Price * quantity)
                : null;
        }

        public async Task<List<(string ProductName, int Quantity, decimal TotalPrice)>> GetAffordableProductsAsync(int storeId, decimal budget)
        {
            var batches = await _productRepository.GetBatchesByStoreIdAsync(storeId);
            var sortedBatches = batches
                .OrderBy(b => b.Price);

            var affordableProducts = new List<(string ProductName, int Quantity, decimal TotalPrice)>();

            foreach (var batch in sortedBatches)
            {
                int maxQuantity = (int)(budget / batch.Price);
                if (maxQuantity == 0) continue;

                int purchasedQuantity = Math.Min(maxQuantity, batch.Quantity);
                affordableProducts.Add((batch.ProductName, purchasedQuantity, purchasedQuantity * batch.Price));
            }

            return affordableProducts;
        }

    }
}
