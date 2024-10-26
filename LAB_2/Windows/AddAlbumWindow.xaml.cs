using MusicCatalog.Entities;
using System;
using System.Linq;
using System.Windows;

namespace MusicCatalog.Windows
{
    public partial class AddAlbumWindow : Window
    {
        private Catalog catalog;

        public AddAlbumWindow(Catalog catalog)
        {
            InitializeComponent();
            this.catalog = catalog;

            foreach (var artist in catalog.GetArtists())
            {
                ArtistComboBox.Items.Add(artist);
            }
        }

        private void OnAddAlbumButtonClick(object sender, RoutedEventArgs e)
        {
            string name = AlbumNameTextBox.Text;
            Artist selectedArtist = ArtistComboBox.SelectedItem as Artist;

            DateTime? releaseDate = ReleaseDatePicker.SelectedDate;

            if (selectedArtist == null)
            {
                MessageBox.Show("Выберите артиста для альбома.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!releaseDate.HasValue)
            {
                MessageBox.Show("Выберите дату выхода альбома.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (releaseDate.Value.Year < selectedArtist.YearOfDebut)
            {
                MessageBox.Show($"Дата выхода альбома не может быть раньше года начала карьеры артиста ({selectedArtist.YearOfDebut}).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Album album = new Album.AlbumBuilder()
                .SetTitle(name)
                .SetArtist(selectedArtist)
                .SetReleaseDate(releaseDate.Value) 
                .Build();

            catalog.AddAlbum(album);

            MessageBox.Show($"Альбом '{name}' добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
