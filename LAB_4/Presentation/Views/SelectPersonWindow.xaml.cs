using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LAB_4.DAL.Models;
using LAB_4.BLL.Interfaces;

namespace LAB_4.Presentation.Views
{
    public partial class SelectPersonWindow : Window
    {
        private readonly IPersonService _personService;
        public Person SelectedPerson { get; private set; }

        public SelectPersonWindow(IPersonService personService)
        {
            InitializeComponent();
            _personService = personService;
            LoadPeople();
        }

        private async void LoadPeople()
        {
            var people = await _personService.GetAllPeopleAsync();
            var displayList = people
                .Select(p => new
                {
                    DisplayText = $"{p.Id}. {p.LastName} {p.FirstName} {p.MiddleName}".Trim(),
                    Person = p
                })
                .OrderBy(p => p.Person.Id)
                .ToList();

            PersonComboBox.ItemsSource = displayList;
            PersonComboBox.DisplayMemberPath = "DisplayText";
            PersonComboBox.SelectedValuePath = "Person";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPerson = (Person)PersonComboBox.SelectedValue;
            DialogResult = SelectedPerson != null;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
