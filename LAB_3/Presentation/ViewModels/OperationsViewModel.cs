using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_3.BLL.Interfaces;
using LAB_3.BLL.Services;
using LAB_3.Models;

namespace LAB_3.Client.ViewModels
{
    public class OperationsViewModel 
    {
        private readonly IBatchService _batchService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        public ObservableCollection<Store> Stores { get; private set; }
        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<Batch> Batches { get; private set; }

        public OperationsViewModel(IStoreService storeService, IBatchService batchService, IProductService productService)
        {
            _storeService = storeService;
            _batchService = batchService;
            _productService = productService;

            Batches = new ObservableCollection<Batch>();
            Stores = new ObservableCollection<Store>();
            Products = new ObservableCollection<Product>();
            Results = new ObservableCollection<Result>();

            _storeService.StoresUpdated += async () => await RefreshStores();
            _productService.ProductsUpdated += async () => await RefreshProducts();
            _batchService.BatchesUpdated += async () => await RefreshBatches();

            _ = RefreshStores();
            _ = RefreshProducts();
            _ = RefreshBatches();
        }
        
        public ObservableCollection<Result> Results { get; private set; }

        public Store SelectedStore { get; set; }
        public Product SelectedProduct { get; set; }
        public Product SelectedBatchProduct { get; set; }
        public Store SelectedBatchStore { get; set; }
        public Product SelectedBatchProductForSearch { get; set; }
        public decimal Budget { get; set; }
        public int BatchQuantity { get; set; }
        public int BatchQuantityForSearch { get; set; }

        public ICommand FindCheapestStoreCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedProduct != null)
            {
                var result = await _storeService.FindCheapestStoreAsync(SelectedProduct.Name);

                if (result.HasValue && result.Value.Store != null)
                {
                    var store = result.Value.Store;
                    var cost = result.Value.cost;

                    Results.Add(new Result
                        {
                            Description = $"Самый дешевый магазин для {SelectedProduct.Name}: {store.Name}, Адрес: {store.Address}, Цена: {cost}"
                        }
                    );
                }
                else Results.Add(new Result
                        {
                            Description = "Магазин не найден."
                        }
                    );
            }
        });

        public ICommand GetAffordableProductsCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedStore != null && Budget > 0)
            { 
                var affordableProducts = await _storeService.GetAffordableProductsAsync(SelectedStore.StoreId, Budget);
                var results = affordableProducts.Select(p => $"{p.ProductName}, Количество: {p.Quantity}, Цена: {p.TotalPrice}").ToList();
                if (results != null)
                {
                    Results.Add(new Result { Description = $"В магазине {SelectedStore.Name} с бюджетом {Budget} можно купить:" });
                    foreach (var result in results)
                    {
                        Results.Add(new Result { Description = result });
                    }
                }
                else
                {
                    Results.Add(new Result { Description = $"В магазине {SelectedStore.Name} с бюджетом {Budget} невозможно купить ничего." });
                }
            }
        });

        public ICommand PurchaseBatchCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedBatchProduct != null && SelectedBatchStore != null && BatchQuantity > 0)
            {
                var result = await _batchService.PurchaseBatchAsync(
                    SelectedBatchStore.StoreId, SelectedBatchProduct.Name, BatchQuantity);

                Results.Add(
                    new Result
                    {
                        Description = result.IsPossible
                            ? $"Покупка {BatchQuantity} {SelectedBatchProduct.Name} в магазине {SelectedBatchStore.Name} успешна, потрачено {result.TotalCost}"
                            : "Недостаточно товара"
                    }
                );
            }
        });


        public ICommand FindCheapestStoreForBatchCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedBatchProductForSearch != null && BatchQuantityForSearch > 0)
            {
                var result = await _storeService.FindStoreWithCheapestBatchAsync(
                    SelectedBatchProductForSearch.Name, BatchQuantityForSearch);

                if (result.HasValue && result.Value.Store != null)
                {
                    var store = result.Value.Store;
                    var cost = result.Value.TotalCost;

                    Results.Add(new Result
                        {
                            Description =  $"Самый дешевый магазин для {SelectedBatchProductForSearch.Name} в количестве {BatchQuantityForSearch}: {store.Name}, Адрес: {store.Address}, Сумма: {cost}"
                        }
                    );
                }
                else Results.Add(new Result
                        {
                            Description = "Магазин не найден."
                        }
                    );
            }
        });
        public ICommand ClearResultsCommand => new RelayCommand(() => Results.Clear());

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

        private async Task RefreshBatches()
        {
            var batchesList = await _batchService.GetAllBatchesAsync();
            Batches.Clear();
            foreach (var batch in batchesList)
            {
                Batches.Add(batch);
            }
        }

    }

    public class Result
    {
        public string Description { get; set; }
    }
}
