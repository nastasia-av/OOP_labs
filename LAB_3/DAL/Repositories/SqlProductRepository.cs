using LAB_3.DAL.Interfaces;
using LAB_3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_3.DAL.Repositories
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByNameAsync(string productName)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Name == productName);
        }

        public async Task<IEnumerable<Product>> GetProductsByStoreIdAsync(int storeId)
        {
            // Получаем все партии с нужным StoreId, затем по их именам находим соответствующие продукты
            var productNames = await _context.Batches
                .Where(b => b.StoreId == storeId)
                .Select(b => b.ProductName)
                .Distinct()
                .ToListAsync();

            return await _context.Products
                .Where(p => productNames.Contains(p.Name))
                .ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            if (!await _context.Products.AnyAsync(p => p.Name == product.Name))
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                existingProduct.Description = product.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductAsync(string productName)
        {
            // Удаляем продукт и все его связанные партии
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == productName);

            if (product != null)
            {
                var batches = await _context.Batches
                    .Where(b => b.ProductName == productName)
                    .ToListAsync();

                _context.Batches.RemoveRange(batches);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Batch>> GetAllBatchesAsync()
        { 
            return await _context.Batches.ToListAsync();
        }

        public async Task<IEnumerable<Batch>> GetBatchesByProductNameAsync(string productName)
        {
            return await _context.Batches
                .Where(b => b.ProductName == productName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Batch>> GetBatchesByStoreIdAsync(int storeId)
        {
            return await _context.Batches
                .Where(b => b.StoreId == storeId)
                .ToListAsync();
        }

        public async Task AddBatchToProductAsync(string productName, Batch batch)
        {
                await _context.Batches.AddAsync(batch);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            var existingBatch = await _context.Batches
                .FirstOrDefaultAsync(b => b.ProductName == batch.ProductName && b.StoreId == batch.StoreId);

            if (existingBatch != null)
            {
                existingBatch.Quantity = batch.Quantity;
                existingBatch.Price = batch.Price;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBatchAsync(string productName, int storeId)
        {
            var batchToRemove = await _context.Batches
                .FirstOrDefaultAsync(b => b.ProductName == productName && b.StoreId == storeId);

            if (batchToRemove != null)
            {
                _context.Batches.Remove(batchToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
}
