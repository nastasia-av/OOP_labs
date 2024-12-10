using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_3.Models;

namespace LAB_3.Client.ViewModels
{
    public class EditStoreViewModel : INotifyPropertyChanged
    {
        private string _storeName;
        private string _storeAddress;

        public string StoreName
        {
            get => _storeName;
            set
            {
                if (_storeName != value)
                {
                    _storeName = value;
                    OnPropertyChanged(nameof(StoreName));
                }
            }
        }

        public string StoreAddress
        {
            get => _storeAddress;
            set
            {
                if (_storeAddress != value)
                {
                    _storeAddress = value;
                    OnPropertyChanged(nameof(StoreAddress));
                }
            }
        }

        public ICommand ConfirmCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<Store>? OnConfirm;

        public EditStoreViewModel(Store store)
        {
            StoreName = store.Name;
            StoreAddress = store.Address;

            ConfirmCommand = new RelayCommand(() =>
            {
                OnConfirm?.Invoke(new Store
                {
                    StoreId = store.StoreId,
                    Name = StoreName,
                    Address = StoreAddress
                });
            });
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
