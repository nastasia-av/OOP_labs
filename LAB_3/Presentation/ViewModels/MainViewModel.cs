using System.Windows.Input;
using LAB_3.BLL.Interfaces;
using LAB_3.BLL.Services;
using LAB_3.Client.Views;

namespace LAB_3.Client.ViewModels
{
    public class MainViewModel
    {
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly IBatchService _batchService;
        public ICommand OpenStoreManagementCommand { get; }
        public ICommand OpenProductManagementCommand { get; }
        public ICommand OpenOperationsCommand { get; }
        public ICommand OpenBatchManagementCommand { get; }

        public MainViewModel(IStoreService storeService, IProductService productService, IBatchService batchService)
        {
            _storeService = storeService;
            _productService = productService;
            _batchService = batchService;
            OpenStoreManagementCommand = new RelayCommand(OpenStoreManagement);
            OpenProductManagementCommand = new RelayCommand(OpenProductManagement);
            OpenOperationsCommand = new RelayCommand(OpenOperations);
            OpenBatchManagementCommand = new RelayCommand(OpenBatchManagement);
        }

        private void OpenStoreManagement()
        {
            var viewModel = new StoreManagementViewModel(_storeService);
            var window = new StoreManagementView();
            window.DataContext = viewModel;
            window.Show();
        }

        private void OpenProductManagement()
        {
            var viewModel = new ProductManagementViewModel(_productService);
            var window = new ProductManagementView();
            window.DataContext = viewModel;
            window.Show();
        }

        private void OpenOperations()
        {
            var viewModel = new OperationsViewModel(_storeService, _batchService, _productService);
            var window = new OperationsView();
            window.DataContext = viewModel;
            window.Show();
        }

        private void OpenBatchManagement()
        {
            var viewModel = new BatchManagementViewModel(_batchService, _storeService, _productService);
            var window = new BatchManagementView();
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
