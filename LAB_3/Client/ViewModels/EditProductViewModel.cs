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
    public class EditProductViewModel : INotifyPropertyChanged
    {
        private string _productDescription;

        public string ProductDescription
        {
            get => _productDescription;
            set
            {
                if (_productDescription != value)
                {
                    _productDescription = value;
                    OnPropertyChanged(nameof(ProductDescription));
                }
            }
        }

        public ICommand ConfirmCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<Product>? OnConfirm;

        public EditProductViewModel(Product product)
        {
            ProductDescription = product.Description;

            ConfirmCommand = new RelayCommand(() =>
            {
                OnConfirm?.Invoke(new Product
                {
                    Name = product.Name,
                    Description = ProductDescription
                });
            });
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
