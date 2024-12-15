using System.ComponentModel;
using System.Windows.Input;
using LAB_4.BLL.Services;
using LAB_4.Presentation.Utils;

namespace LAB_4.Presentation.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly GenealogyTreeService _service;
        private string _treeText;

        public string TreeText
        {
            get => _treeText;
            set { _treeText = value; OnPropertyChanged(nameof(TreeText)); }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand ShowTreeCommand { get; }
        public ICommand SaveCommand { get; }

        public MainViewModel(GenealogyTreeService service)
        {
            _service = service;

            AddPersonCommand = new RelayCommand(AddPerson);
            ShowTreeCommand = new RelayCommand(ShowTree);
            SaveCommand = new RelayCommand(SaveTree);
        }

        private void AddPerson() { /* Открытие формы добавления */ }
        private void ShowTree() { TreeText = _service.FormatTree(); }
        private void SaveTree() { _service.SaveTree(); }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
