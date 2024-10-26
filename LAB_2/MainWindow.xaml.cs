using MusicCatalog.Entities;
using MusicCatalog.SearchStrategies;
using MusicCatalog.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace MusicCatalog
{
    public partial class MainWindow : Window
    {
        Catalog catalog = new Catalog();
        private ISearchStrategy searchStrategy;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string filter = selectedItem.Content.ToString();

                // Управление видимостью дополнительных полей для поиска треков
                if (filter == "Track")
                {
                    TrackSearchCriteriaComboBox.Visibility = Visibility.Visible;
                    SearchBoxPlaceholder.Text = "Введите название";
                }
                else
                {
                    TrackSearchCriteriaComboBox.Visibility = Visibility.Collapsed;
                    DurationRangePanel.Visibility = Visibility.Collapsed;
                    SearchBox.Visibility = Visibility.Visible;
                    GenreComboBox.Visibility = Visibility.Collapsed;
                    SearchBoxPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void OnTrackCriteriaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TrackSearchCriteriaComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string criteria = selectedItem.Content.ToString();

                switch (criteria)
                {
                    case "По названию":
                        SearchBoxPlaceholder.Text = "Введите название";
                        SearchBox.Visibility = Visibility.Visible; 
                        SearchBoxPlaceholder.Visibility = Visibility.Visible;
                        GenreComboBox.Visibility = Visibility.Collapsed; 
                        DurationRangePanel.Visibility = Visibility.Collapsed; 
                        break;

                    case "По жанру":
                        SearchBox.Visibility = Visibility.Collapsed; 
                        SearchBoxPlaceholder.Visibility = Visibility.Collapsed; 
                        GenreComboBox.Visibility = Visibility.Visible; 
                        DurationRangePanel.Visibility = Visibility.Collapsed; 
                        break;

                    case "По длительности":
                        SearchBox.Visibility = Visibility.Collapsed; 
                        SearchBoxPlaceholder.Visibility = Visibility.Collapsed; 
                        GenreComboBox.Visibility = Visibility.Collapsed; 
                        DurationRangePanel.Visibility = Visibility.Visible; 
                        break;
                }
            }
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBoxPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBoxPlaceholder.Visibility = string.IsNullOrEmpty(SearchBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, была ли нажата клавиша Enter
            if (e.Key == Key.Enter)
            {
                ExecuteSearch(); // Запускаем поиск
            }
        }

        private void DurationFromMinutesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DurationFromMinutesPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void DurationFromMinutesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DurationFromMinutesPlaceholder.Visibility = string.IsNullOrEmpty(DurationFromMinutesTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void DurationFromSecondsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DurationFromSecondsPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void DurationFromSecondsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DurationFromSecondsPlaceholder.Visibility = string.IsNullOrEmpty(DurationFromSecondsTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        // Для текстбокса "До" (минуты и секунды)
        private void DurationToMinutesTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DurationToMinutesPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void DurationToMinutesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DurationToMinutesPlaceholder.Visibility = string.IsNullOrEmpty(DurationToMinutesTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void DurationToSecondsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DurationToSecondsPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void DurationToSecondsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            DurationToSecondsPlaceholder.Visibility = string.IsNullOrEmpty(DurationToSecondsTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }


        private void OnSearchButtonClick(object sender, RoutedEventArgs e)
        {
            ExecuteSearch();
        }

        private void OnAddArtistButtonClick(object sender, RoutedEventArgs e)
        {
            AddArtistWindow addArtistWindow = new AddArtistWindow(catalog);
            addArtistWindow.ShowDialog();
        }

        private void OnAddAlbumButtonClick(object sender, RoutedEventArgs e)
        {
            AddAlbumWindow addAlbumWindow = new AddAlbumWindow(catalog);
            addAlbumWindow.ShowDialog();
        }

        private void OnAddTrackButtonClick(object sender, RoutedEventArgs e)
        {
            AddTrackWindow addTrackWindow = new AddTrackWindow(catalog);
            addTrackWindow.ShowDialog();
        }

        private void ExecuteSearch()
        {
            ResultsStackPanel.Children.Clear(); 

            string filterType = (FilterComboBox.SelectedItem as ComboBoxItem)?.Content as string;
            object criteria = SearchBox.Text;

            SearchType searchType;
            switch (filterType)
            {
                case "Artist":
                    searchType = SearchType.Artist;
                    break;
                case "Album":
                    searchType = SearchType.Album;
                    break;
                case "Track":
                    searchType = SearchType.Track;
                    break;
                default:
                    searchType = SearchType.All;
                    break;
            }

            switch (searchType)
            {
                case SearchType.Artist:
                case SearchType.Album:
                    searchStrategy = new NameSearchStrategy();
                    break;
                case SearchType.Track:
                    searchStrategy = GetTrackSearchStrategy();
                    criteria = GetTrackSearchCriteria();
                    break;
                default:
                    searchStrategy = new NameSearchStrategy();
                    break;
            }

            // Выполняем поиск и отображаем результаты
            var results = searchStrategy.Search(catalog, criteria, searchType);
            DisplayResults(results);
        }

        private ISearchStrategy GetTrackSearchStrategy()
        {
            string trackCriteria = (TrackSearchCriteriaComboBox.SelectedItem as ComboBoxItem)?.Content as string;
            ISearchStrategy searchStrategy;
            switch (trackCriteria)
            {
                case "По жанру":
                    searchStrategy = new GenreSearchStrategy();
                    break;
                case "По длительности":
                    searchStrategy = new DurationSearchStrategy();
                    break;
                default:
                    searchStrategy = new NameSearchStrategy();
                    break;
            }
            return searchStrategy;
        }

        private object GetTrackSearchCriteria()
        {
            string trackCriteria = (TrackSearchCriteriaComboBox.SelectedItem as ComboBoxItem)?.Content as string;

            object searchCriteria;
            switch (trackCriteria)
            {
                case "По длительности":
                    searchCriteria = GetDurationRange();
                    break;
                case "По жанру":
                    searchCriteria = (GenreComboBox.SelectedItem as ComboBoxItem)?.Content as string;
                    break;
                default:
                    searchCriteria = SearchBox.Text;
                    break;
            }
            return searchCriteria;

        }

        private Tuple<TimeSpan, TimeSpan> GetDurationRange()
        {
            if (!ValidateDuration(DurationFromMinutesTextBox.Text, DurationFromSecondsTextBox.Text, out TimeSpan minDuration) ||
                !ValidateDuration(DurationToMinutesTextBox.Text, DurationToSecondsTextBox.Text, out TimeSpan maxDuration))
            {
                return null; // Или обработайте ошибку по вашему усмотрению
            }

            return new Tuple<TimeSpan, TimeSpan>(minDuration, maxDuration);
        }

        private bool ValidateDuration(string minutesInput, string secondsInput, out TimeSpan duration)
        {
            duration = TimeSpan.Zero;

            if (string.IsNullOrWhiteSpace(minutesInput) || string.IsNullOrWhiteSpace(secondsInput)) return false;

            if (!int.TryParse(minutesInput, out int minutes) || !int.TryParse(secondsInput, out int seconds))
            {
                return false;
            }

            if (minutes < 0 || seconds < 0 || seconds >= 60)
            {
                return false;
            }

            duration = new TimeSpan(0, minutes, seconds);
            return true;
        }

        private void DisplayResults(List<object> results)
        {
            ResultsStackPanel.Children.Clear(); 

            foreach (var result in results)
            {
                TextBlock textBlock = new TextBlock { Margin = new Thickness(40, 5, 5, 5) };

                switch (result)
                {
                    case Artist artist:
                        textBlock.Text = $"Имя: {artist.Name}, Год начала карьеры: {artist.YearOfDebut}";
                        break;
                    case Album album:
                        textBlock.Text = $"Название: {album.Title}, Исполнитель: {album.Artist.Name}, Дата выхода: {album.ReleaseDate.ToShortDateString()}";
                        break;
                    case Track track:
                        textBlock.Text = $"Название: {track.Title}, Альбом: {track.Album.Title}, Жанр: {track.Genre}, Длительность: {track.Duration}";
                        break;
                    default:
                        textBlock.Text = "Неизвестный тип результата";
                        break;
                }

                ResultsStackPanel.Children.Add(textBlock);
            }
        }

    }
}
