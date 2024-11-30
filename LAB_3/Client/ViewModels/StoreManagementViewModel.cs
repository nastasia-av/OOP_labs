using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_3.BLL.Interfaces;
using LAB_3.BLL.Services;
using LAB_3.Client.Views;
using LAB_3.Models;

namespace LAB_3.Client.ViewModels
{
    public class StoreManagementViewModel 
    {
        private readonly IStoreService _storeService;
        public ObservableCollection<Store> Stores { get; private set; }
        public StoreManagementViewModel(IStoreService storeService)
        {
            _storeService = storeService;
            Stores = new ObservableCollection<Store>();
            RefreshStores();
        }
        public string NewStoreName { get; set; }
        public string NewStoreAddress { get; set; }

        public ICommand AddStoreCommand => new AsyncRelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(NewStoreName) && !string.IsNullOrEmpty(NewStoreAddress))
            {
                var newStore = new Store {Name = NewStoreName, Address = NewStoreAddress };
                await _storeService.AddStoreAsync(newStore);
                RefreshStores();
            }
        });

        public ICommand EditStoreCommand => new RelayCommand<Store>(store =>
        {
            var editViewModel = new EditStoreViewModel(store);
            var editWindow = new EditStoreWindow { DataContext = editViewModel };

            editViewModel.OnConfirm += async updatedStore =>
            {
                await _storeService.UpdateStoreAsync(updatedStore);
                RefreshStores();
                editWindow.Close();
            };

            editWindow.ShowDialog();
        });


        public ICommand DeleteStoreCommand => new AsyncRelayCommand<Store>(async store =>
        {
            if (store != null)
            {
                await _storeService.DeleteStoreAsync(store.StoreId);
                RefreshStores();
            }
        });

        private async void RefreshStores()
        {
            var storesList = await _storeService.GetAllStoresAsync();
            Stores.Clear();
            foreach (var store in storesList)
            {
                Stores.Add(store);
            }
        }

    }

}
