using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_3.BLL.Services;
using LAB_3.Models;
using System.ComponentModel;
using LAB_3.BLL.Interfaces;
using LAB_3.Client.Views;

namespace LAB_3.Client.ViewModels
{
    public class BatchManagementViewModel
    {
        private readonly IBatchService _batchService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        public ObservableCollection<Batch> Batches { get; private set; }

        public BatchManagementViewModel(IBatchService batchService, IStoreService storeService, IProductService productService)
        {
            _batchService = batchService;
            _storeService = storeService;
            _productService = productService;
            Batches = new ObservableCollection<Batch>();
            Stores = new ObservableCollection<Store>();
            Products = new ObservableCollection<Product>();

            _storeService.StoresUpdated += async () => await RefreshStores();
            _productService.ProductsUpdated += async () => await RefreshProducts();

            _ = RefreshStores();
            _ = RefreshProducts();
            RefreshBatches();
        }
        public ObservableCollection<Store> Stores { get; private set; }
        public ObservableCollection<Product> Products { get; private set; }

        public Store SelectedStore { get; set; }
        public Store SelectedStoreForSearch { get; set; }
        public Product SelectedProduct { get; set; }
        public Product SelectedProductForSearch { get; set; }
        public int NewQuantity { get; set; }
        public decimal NewPrice { get; set; }

        public ICommand AddBatchCommand => new RelayCommand(() =>
        {
            if (SelectedStore != null && SelectedProduct != null && NewQuantity > 0 && NewPrice > 0)
            {
                var batch = new Batch() { StoreId = SelectedStore.StoreId, ProductName = SelectedProduct.Name, Quantity = NewQuantity, Price = NewPrice };
                _batchService.AddBatchAsync(SelectedProduct.Name, batch);
                RefreshBatches();
            }
        });

        public ICommand FindProductInStoresCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedProductForSearch != null)
            {
                var batchesList = await _batchService.GetBatchesByProductNameAsync(SelectedProductForSearch.Name);
                Batches.Clear();
                foreach (var batch in batchesList)
                {
                    Batches.Add(batch);
                }
            }
        });

        public ICommand GetAllProductsInStoreCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedStoreForSearch != null)
            {
                var batchesList = await _batchService.GetBatchesByStoreIdAsync(SelectedStoreForSearch.StoreId);
                Batches.Clear();
                foreach (var batch in batchesList)
                {
                    Batches.Add(batch);
                }
            }
        });

        public ICommand ShowAllBatchesCommand => new RelayCommand(() => RefreshBatches());

        public ICommand EditBatchCommand => new RelayCommand<Batch>(batch =>
        {
            var editViewModel = new EditBatchViewModel(batch);
            var editWindow = new EditBatchWindow { DataContext = editViewModel };

            editViewModel.OnConfirm += async updatedBatch =>
            {
                await _batchService.UpdateBatchAsync(updatedBatch);
                RefreshBatches();
                editWindow.Close();
            };

            editWindow.ShowDialog();
        });


        public ICommand DeleteBatchCommand => new RelayCommand<Batch>(batch =>
        {
            if (batch != null)
            {
                _batchService.DeleteBatchAsync(batch.ProductName, batch.StoreId);
                RefreshBatches();
            }
        });

        private async Task RefreshProducts()
        {
            var productsList = await _productService.GetAllProductsAsync();
            Products.Clear();
            foreach (var product in productsList)
            {
                Products.Add(product);
            }
        }

        private async Task RefreshStores()
        {
            var storesList = await _storeService.GetAllStoresAsync();
            Stores.Clear();
            foreach (var store in storesList)
            {
                Stores.Add(store);
            }
        }

        private async void RefreshBatches()
        {
            var batchesList = await _batchService.GetAllBatchesAsync();
            Batches.Clear();
            foreach (var batch in batchesList)
            {
                Batches.Add(batch);
            }
        }
    }

}
