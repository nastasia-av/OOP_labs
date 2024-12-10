using System.Windows;
using LAB_3.Client.ViewModels;

namespace LAB_3.Client
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
