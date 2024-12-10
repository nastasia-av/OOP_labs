using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_3.BLL.Interfaces;
using LAB_3.Client.Views;
using LAB_3.Models;

namespace LAB_3.Client.ViewModels
{
    public class ProductManagementViewModel
    {
        private readonly IProductService _productService;
        public ObservableCollection<Product> Products { get; private set; }
        public ProductManagementViewModel(IProductService productService)
        {
            _productService = productService;
            Products = new ObservableCollection<Product>();
            RefreshProducts();
        }
        public string NewProductName { get; set; }
        public string NewProductDescription { get; set; }

        public ICommand AddProductCommand => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(NewProductName) && !string.IsNullOrEmpty(NewProductDescription))
            {
                var newProduct = new Product() { Name = NewProductName, Description = NewProductDescription };
                _productService.AddProductAsync(newProduct);
                RefreshProducts();
            }
        });

        public ICommand EditProductCommand => new RelayCommand<Product>(product =>
        {
            var editViewModel = new EditProductViewModel(product);
            var editWindow = new EditProductWindow { DataContext = editViewModel };

            editViewModel.OnConfirm += async updatedProduct =>
            {
                await _productService.UpdateProductAsync(updatedProduct);
                RefreshProducts();
                editWindow.Close();
            };

            editWindow.ShowDialog();
        });


        public ICommand DeleteProductCommand => new RelayCommand<Product>(product =>
        {
            if (product != null)
            {
                _productService.DeleteProductAsync(product.Name);
                RefreshProducts();
            }
        });

        private async void RefreshProducts()
        {
            var productsList = await _productService.GetAllProductsAsync();
            Products.Clear();
            foreach (var product in productsList)
            {
                Products.Add(product);
            }
        }
    }

}
