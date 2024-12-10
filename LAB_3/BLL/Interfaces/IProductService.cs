using LAB_3.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LAB_3.BLL.Interfaces
{
    public interface IProductService
    {
        event Action ProductsUpdated;
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByNameAsync(string productName);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string productName);
    }
}
