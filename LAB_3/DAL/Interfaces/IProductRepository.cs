using LAB_3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAB_3.DAL.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByNameAsync(string productName);
        Task<IEnumerable<Product>> GetProductsByStoreIdAsync(int storeId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string productName);
        Task<IEnumerable<Batch>> GetAllBatchesAsync();
        Task<IEnumerable<Batch>> GetBatchesByProductNameAsync(string productName);
        Task<IEnumerable<Batch>> GetBatchesByStoreIdAsync(int storeId);
        Task AddBatchToProductAsync(string productName, Batch batch);
        Task UpdateBatchAsync(Batch batch);
        Task DeleteBatchAsync(string productName, int storeId);
    }

}
