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
    public class EditBatchViewModel : INotifyPropertyChanged
    {
        private int _quantity;
        private decimal _price;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public ICommand ConfirmCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<Batch>? OnConfirm;

        public EditBatchViewModel(Batch batch)
        {
            Quantity = batch.Quantity;
            Price = batch.Price;

            ConfirmCommand = new RelayCommand(() =>
            {
                OnConfirm?.Invoke(new Batch
                {
                    StoreId = batch.StoreId,
                    ProductName = batch.ProductName,
                    Quantity = Quantity,
                    Price = Price
                });
            });
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
