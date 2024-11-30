using LAB_3.DAL.Interfaces;
using LAB_3.BLL.Interfaces;
using LAB_3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_3.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public event Action? ProductsUpdated;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return (await _productRepository.GetAllProductsAsync()).ToList();
        }

        public async Task<Product?> GetProductByNameAsync(string productName)
        {
            return await _productRepository.GetProductByNameAsync(productName);
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
            ProductsUpdated?.Invoke();
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProductAsync(product);
            ProductsUpdated?.Invoke();
        }

        public async Task DeleteProductAsync(string productName)
        {
            var batches = await _productRepository.GetBatchesByProductNameAsync(productName);
            foreach (var batch in batches)
            {
                await _productRepository.DeleteBatchAsync(productName, batch.StoreId);
            }
            await _productRepository.DeleteProductAsync(productName);
            ProductsUpdated?.Invoke();
        }
    }
}
