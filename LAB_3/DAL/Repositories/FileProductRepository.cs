using LAB_3.DAL.Interfaces;
using LAB_3.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_3.DAL.Repositories
{
    public class FileProductRepository : IProductRepository
    {
        private readonly string _filePath;

        public FileProductRepository(string filePath)
        {
            _filePath = Path.Combine(Environment.CurrentDirectory, filePath); 
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var data = await ParseFileAsync();
            return data.Select(d => d.Product);
        }

        public async Task<Product?> GetProductByNameAsync(string productName)
        {
            var data = await ParseFileAsync();
            var productData = data.FirstOrDefault(d => d.Product.Name == productName);
            return productData.Product != null ? productData.Product : null;
        }

        public async Task<IEnumerable<Product>> GetProductsByStoreIdAsync(int storeId)
        {
            var data = await ParseFileAsync();
            return data
                .Where(d => d.Batches.Any(b => b.StoreId == storeId))
                .Select(d => d.Product);
        }

        public async Task AddProductAsync(Product product)
        {
            var data = (await ParseFileAsync()).ToList();
            if (data.Any(d => d.Product.Name == product.Name)) return; // Продукт уже существует

            data.Add((product, new List<Batch>()));
            await SaveAllProductsAsync(data);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var data = (await ParseFileAsync()).ToList();
            var existingData = data.FirstOrDefault(d => d.Product.Name == product.Name);
            if (existingData.Product != null)
            {
                existingData.Product.Description = product.Description;
                await SaveAllProductsAsync(data);
            }
        }

        public async Task DeleteProductAsync(string productName)
        {
            var data = (await ParseFileAsync()).ToList();
            data.RemoveAll(d => d.Product.Name == productName);
            await SaveAllProductsAsync(data);
        }

        public async Task<IEnumerable<Batch>> GetAllBatchesAsync()
        {
            var data = await ParseFileAsync();
            return data
                .SelectMany(d => d.Batches);
        }

        public async Task<IEnumerable<Batch>> GetBatchesByProductNameAsync(string productName)
        {
            var data = await ParseFileAsync();
            var productData = data.FirstOrDefault(d => d.Product.Name == productName);
            return productData.Product != null ? productData.Batches : new List<Batch>();
        }

        public async Task<IEnumerable<Batch>> GetBatchesByStoreIdAsync(int storeId)
        {
            var data = await ParseFileAsync();
            return data
                .SelectMany(d => d.Batches)
                .Where(b => b.StoreId == storeId);
        }

        public async Task AddBatchToProductAsync(string productName, Batch batch)
        {
            var data = (await ParseFileAsync()).ToList();
            var productData = data.FirstOrDefault(d => d.Product.Name == productName);
            if (productData.Product != null)
            {
                productData.Batches.Add(batch);
                await SaveAllProductsAsync(data);
            }
        }

        public async Task UpdateBatchAsync(Batch batch)
        {
            var data = (await ParseFileAsync()).ToList();
            foreach (var productData in data)
            {
                var existingBatch = productData.Batches
                    .FirstOrDefault(b => b.StoreId == batch.StoreId && b.ProductName == batch.ProductName);

                if (existingBatch != null)
                {
                    existingBatch.Quantity = batch.Quantity;
                    existingBatch.Price = batch.Price;
                    await SaveAllProductsAsync(data);
                    return;
                }
            }
        }

        public async Task DeleteBatchAsync(string productName, int storeId)
        {
            var data = (await ParseFileAsync()).ToList();
            var productData = data.FirstOrDefault(d => d.Product.Name == productName);
            if (productData.Product != null)
            {
                productData.Batches.RemoveAll(b => b.StoreId == storeId);
                await SaveAllProductsAsync(data);
            }
        }

        // Парсинг файла
        private async Task<IEnumerable<(Product Product, List<Batch> Batches)>> ParseFileAsync()
        {
            if (!File.Exists(_filePath)) return new List<(Product, List<Batch>)>();

            var lines = await File.ReadAllLinesAsync(_filePath);
            var products = new List<(Product, List<Batch>)>();

            Product? currentProduct = null;
            var currentBatches = new List<Batch>();

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length == 2)
                {
                    if (currentProduct != null)
                    {
                        products.Add((currentProduct, new List<Batch>(currentBatches)));
                        currentBatches.Clear();
                    }

                    currentProduct = new Product
                    {
                        Name = parts[0],
                        Description = parts[1]
                    };
                }
                else if (parts.Length == 3 && currentProduct != null)
                {
                    if (int.TryParse(parts[0], out int storeId) &&
                        int.TryParse(parts[1], out int quantity) &&
                        decimal.TryParse(parts[2], out decimal price))
                    {
                        currentBatches.Add(new Batch
                        {
                            StoreId = storeId,
                            ProductName = currentProduct.Name,
                            Quantity = quantity,
                            Price = price
                        });
                    }
                }
            }

            if (currentProduct != null)
            {
                products.Add((currentProduct, currentBatches));
            }

            return products;
        }

        // Сохранение файла
        private async Task SaveAllProductsAsync(IEnumerable<(Product Product, List<Batch> Batches)> data)
        {
            var lines = new List<string>();
            foreach (var (product, batches) in data)
            {
                lines.Add($"{product.Name},{product.Description}");
                lines.AddRange(batches.Select(b => $"{b.StoreId},{b.Quantity},{b.Price}"));
            }

            await File.WriteAllLinesAsync(_filePath, lines, Encoding.UTF8);
        }
    }
}
