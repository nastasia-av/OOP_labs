using MusicCatalog.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MusicCatalog.Windows
{
    public partial class AddTrackWindow : Window
    {
        private Catalog catalog;

        public AddTrackWindow(Catalog catalog)
        {
            InitializeComponent();
            this.catalog = catalog;

            var albums = this.catalog.GetAlbums();
            foreach (var album in albums)
            {
                AlbumComboBox.Items.Add(album);
            }
        }

        private void OnAddTrackButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TrackNameTextBox.Text) ||
                AlbumComboBox.SelectedItem == null ||
                !TryGetDuration(out TimeSpan duration))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            Album selectedAlbum = AlbumComboBox.SelectedItem as Album;
            string selectedGenre = (GenreComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var newTrack = new Track.TrackBuilder()
                .SetTitle(TrackNameTextBox.Text)
                .SetDuration(duration)
                .SetAlbum(selectedAlbum)
                .SetGenre(selectedGenre)
                .Build();
            catalog.AddTrack(newTrack);

            MessageBox.Show("Трек успешно добавлен!");
            Close();
        }

        private bool TryGetDuration(out TimeSpan duration)
        {
            duration = TimeSpan.Zero;

            if (int.TryParse(DurationMinutesTextBox.Text, out int minutes) &&
                int.TryParse(DurationSecondsTextBox.Text, out int seconds))
            {
                duration = new TimeSpan(0, minutes, seconds);
                return true;
            }

            return false;
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
    }
}
