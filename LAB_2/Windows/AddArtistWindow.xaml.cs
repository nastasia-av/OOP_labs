using MusicCatalog.Entities;
using System.Windows;

namespace MusicCatalog.Windows
{
    public partial class AddArtistWindow : Window
    {
        private Catalog catalog;

        public AddArtistWindow(Catalog catalog)
        {
            InitializeComponent();
            this.catalog = catalog;
        }

        private void OnAddArtistButtonClick(object sender, RoutedEventArgs e)
        {
            string name = ArtistNameTextBox.Text;
            if (!int.TryParse(CareerStartYearTextBox.Text, out int careerStartYear) ||
                careerStartYear < 1900 ||
                careerStartYear > 2024)
            {
                MessageBox.Show("Введите корректный год начала карьеры (1900 - текущий год).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Artist artist = new Artist.ArtistBuilder().SetName(name).SetYear(careerStartYear).Build();
            catalog.AddArtist(artist); 

            MessageBox.Show($"Артист '{name}' добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
    }
}
