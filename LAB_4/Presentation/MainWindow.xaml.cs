using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using LAB_4.DAL.Models.Enums;
using LAB_4.DAL.Models;
using LAB_4.BLL.Interfaces;
using LAB_4.Presentation.ViewModels;
using LAB_4.Presentation.Views;

namespace LAB_4.Presentation
{
    public partial class MainWindow : Window
    {
        private readonly IPersonService _personService;
        private readonly ITreeService _treeService;
        private readonly IRelationService _relationService;

        private List<Person> _currentPeople = new();
        private List<Relation> _currentRelations = new();
        private Dictionary<int, Point> personPositions = new Dictionary<int, Point>();

        public MainWindow(IPersonService personService, ITreeService treeService, IRelationService relationService)
        {
            InitializeComponent();
            _personService = personService;
            _treeService = treeService;
            _relationService = relationService;
            LoadTree();
        }

        private async void LoadTree()
        {
            TreeCanvas.Children.Clear();
            personPositions.Clear();

            _currentPeople = await _treeService.GetMainTreePeopleAsync();
            if (_currentPeople.Count == 0) return;

            _currentRelations = await _relationService.GetRelationsForPeopleAsync(_currentPeople.Select(p => p.Id).ToList());

            DrawTree(_currentPeople, _currentRelations);
        }

        private Dictionary<int, int> CalculateLevels(List<Person> people, List<Relation> relations)
        {
            Dictionary<int, int> levels = new Dictionary<int, int>();

            foreach (var person in people)
            {
                levels[person.Id] = 0;
            }

            foreach (var relation in relations)
            {
                if (relation.RelationType == RelationType.Parent && !levels.ContainsKey(relation.PersonId))
                {
                    levels[relation.PersonId] = 0; 
                }

                if (relation.RelationType == RelationType.Child && !levels.ContainsKey(relation.PersonId))
                {
                    levels[relation.PersonId] = 0;
                }

                if (relation.RelationType == RelationType.Sibling && !levels.ContainsKey(relation.PersonId))
                {
                    levels[relation.PersonId] = 0;
                }
            }

            bool updated;
            do
            {
                updated = false;

                foreach (var relation in relations)
                {
                    if (relation.RelationType == RelationType.Parent)
                    {
                        int parentLevel = levels[relation.PersonId];
                        int childLevel = levels[relation.RelatedPersonId];

                        if (parentLevel <= childLevel)
                        {
                            levels[relation.PersonId] = childLevel + 1;
                            updated = true;
                        }
                    }

                    if (relation.RelationType == RelationType.Sibling)
                    {
                        int personLevel = levels[relation.PersonId];
                        int siblingLevel = levels[relation.RelatedPersonId];
                        if (personLevel != siblingLevel)
                        {
                            int maxLevel = Math.Max(personLevel, siblingLevel);
                            levels[relation.PersonId] = maxLevel;
                            levels[relation.RelatedPersonId] = maxLevel;
                            updated = true;
                        }
                    }
                }
            } while (updated);

            return levels;
        }

        private void DrawTree(List<Person> people, List<Relation> relations)
        {
            TreeCanvas.Children.Clear();
            personPositions.Clear(); 

            if (people == null || people.Count == 0) return;

            Dictionary<int, int> levels = CalculateLevels(people, relations);
            Dictionary<int, int> levelCounts = new Dictionary<int, int>();

            double startX = 200;  
            double startY = 400;
            double xOffset = 200;
            double yOffset = 100;  
            double spouseOffset = 200;

            Dictionary<int, int> siblingCount = new Dictionary<int, int>();

            foreach (var person in people)
            {
                int level = levels[person.Id];

                if (!levelCounts.ContainsKey(level))
                {
                    levelCounts[level] = 0;
                }

                double x = startX + levelCounts[level] * xOffset;
                double y = startY - levels[person.Id] * yOffset;
                levelCounts[level]++;

                if (!personPositions.ContainsKey(person.Id))
                {
                    personPositions[person.Id] = new Point(x, y);
                    DrawPerson(person, x, y);
                }

                var spouseRelation = relations.FirstOrDefault(r =>
                    r.PersonId == person.Id && r.RelationType == RelationType.Spouse);

                if (spouseRelation != null && !personPositions.ContainsKey(spouseRelation.RelatedPersonId))
                {
                    var spouse = people.FirstOrDefault(p => p.Id == spouseRelation.RelatedPersonId);

                    if (spouse != null)
                    {
                        double spouseX = x + spouseOffset;
                        personPositions[spouseRelation.RelatedPersonId] = new Point(spouseX, y);
                        DrawPerson(spouse, spouseX, y);
                    }
                }
            }

            DrawRelations(relations);

        }

        private void DrawRelations(List<Relation> relations)
        {
            TreeCanvas.Children
                .OfType<Line>()
                .ToList()
                .ForEach(line => TreeCanvas.Children.Remove(line));

            foreach (var relation in relations)
            {
                if (personPositions.TryGetValue(relation.PersonId, out var start) &&
                    personPositions.TryGetValue(relation.RelatedPersonId, out var end))
                {
                    DrawLine(start, end);
                }
            }
        }

        private void DrawPerson(Person person, double x, double y)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 60,
                Height = 60,
                Fill = Brushes.LightBlue,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            TextBlock textBlock = new TextBlock
            {
                Text = $"{person.LastName} {person.FirstName} {person.MiddleName}",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold,
                Background = Brushes.White 
            };

            textBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            textBlock.Arrange(new Rect(textBlock.DesiredSize));
            double textWidth = textBlock.ActualWidth;

            Canvas.SetLeft(ellipse, x - ellipse.Width / 2);
            Canvas.SetTop(ellipse, y - ellipse.Height / 2);

            double textX = x - textWidth / 2;
            double textY = y + ellipse.Height / 2 + 5;
            Canvas.SetLeft(textBlock, textX);
            Canvas.SetTop(textBlock, textY);

            Canvas.SetZIndex(ellipse, 0); 
            Canvas.SetZIndex(textBlock, 1);

            TreeCanvas.Children.Add(ellipse);
            TreeCanvas.Children.Add(textBlock);
        }

        private void DrawLine(Point start, Point end)
        {
            Line line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };

            TreeCanvas.Children.Add(line);
        }

        private void ApplyFilter(object sender, RoutedEventArgs e)
        {
            if (_currentPeople == null || _currentRelations == null) return;

            var selectedFilter = (RelationFilter.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrEmpty(selectedFilter)) return;

            List<RelationType> allowedTypes = selectedFilter switch
            {
                "Ближайшие (супруги, дети, родители)" => new List<RelationType> { RelationType.Spouse, RelationType.Child, RelationType.Parent },
                "Все связи" => Enum.GetValues(typeof(RelationType)).Cast<RelationType>().ToList(),
                "Бабушки/Дедушки" => new List<RelationType> { RelationType.Grandparent },
                "Внуки" => new List<RelationType> { RelationType.Grandchild },
                "Братья/Сестры" => new List<RelationType> { RelationType.Sibling },
                "Дяди/Тети" => new List<RelationType> { RelationType.AuntUncle },
                "Племянники" => new List<RelationType> { RelationType.NephewNiece },
                "Кузены" => new List<RelationType> { RelationType.Cousin },
                "Тесть/Теща/Свекр/Свекровь" => new List<RelationType> { RelationType.ParentInLaw },
                "Пасынок/Падчерица" => new List<RelationType> { RelationType.ChildInLaw },
                "Золовка/Шурин/Свояк" => new List<RelationType> { RelationType.SiblingInLaw },
                _ => new List<RelationType>()
            };

            var filteredRelations = _currentRelations.Where(r => allowedTypes.Contains(r.RelationType)).ToList();

            DrawRelations(filteredRelations);
        }

        
        private void OpenPeopleWindow(object sender, RoutedEventArgs e)
        {
            var viewModel = new PersonManagementViewModel(_personService);
            var peopleWindow = new PersonManagementView();
            peopleWindow.DataContext = viewModel;
            peopleWindow.ShowDialog();
            LoadTree();
        }

        private void OpenRelationsWindow(object sender, RoutedEventArgs e)
        {
            var viewModel = new RelationManagementViewModel(_relationService, _personService);
            var relationsWindow = new RelationManagementView();
            relationsWindow.DataContext = viewModel;
            relationsWindow.ShowDialog();
            LoadTree();
        }

        private void OpenFunctionsWindow(object sender, RoutedEventArgs e)
        {
            var viewModel = new OperationsViewModel(_relationService, _personService);
            var functionsWindow = new OperationsView();
            functionsWindow.DataContext = viewModel;
            functionsWindow.ShowDialog();
        }

        private async void AddPersonToTree(object sender, RoutedEventArgs e)
        {
            var selectPersonWindow = new SelectPersonWindow(_personService);
            if (selectPersonWindow.ShowDialog() == true)
            {
                var selectedPerson = selectPersonWindow.SelectedPerson;
                if (selectedPerson != null)
                {
                    await _treeService.AddPersonToTreeAsync(selectedPerson.Id);
                    LoadTree();
                }
            }
        }

        private async void RemovePersonFromTree(object sender, RoutedEventArgs e)
        {
            var selectPersonWindow = new SelectPersonWindow(_personService);
            if (selectPersonWindow.ShowDialog() == true)
            {
                var selectedPerson = selectPersonWindow.SelectedPerson;
                if (selectedPerson != null)
                {
                    _currentRelations.RemoveAll(r => r.PersonId == selectedPerson.Id || r.RelatedPersonId == selectedPerson.Id);

                    if (personPositions.ContainsKey(selectedPerson.Id))
                    {
                        personPositions.Remove(selectedPerson.Id);
                    }
                    await _treeService.RemovePersonFromTreeAsync(selectedPerson.Id);
                    LoadTree();
                }
            }
        }

        private void ResetFilters(object sender, RoutedEventArgs e)
        {
            LoadTree();
        }

        private async void ResetTree(object sender, RoutedEventArgs e)
        {
            await _treeService.CreateNewTreeAsync();
            LoadTree();
        }
    }
}
