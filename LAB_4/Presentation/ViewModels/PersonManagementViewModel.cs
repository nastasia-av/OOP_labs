using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_4.BLL.Interfaces;
using LAB_4.Presentation.Views;
using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;
using LAB_4.Presentation.Utils;


namespace LAB_4.Presentation.ViewModels
{
    public class PersonManagementViewModel
    {
        private readonly IPersonService _personService;
        public ObservableCollection<Person> Persons { get; private set; }

        public PersonManagementViewModel(IPersonService personService)
        {
            _personService = personService;
            Persons = new ObservableCollection<Person>();
            RefreshPersons();
        }

        public string NewPersonFirstName { get; set; }
        public string NewPersonLastName { get; set; }
        public string NewPersonMiddleName { get; set; }
        public DateTime NewPersonDateOfBirth { get; set; } = DateTime.Now;
        public Gender NewPersonGender { get; set; }

        public ObservableCollection<Gender> Genders { get; set; } = new ObservableCollection<Gender>((Gender[])Enum.GetValues(typeof(Gender)));

        public ICommand AddPersonCommand => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(NewPersonFirstName) && !string.IsNullOrEmpty(NewPersonLastName))
            {
                var newPerson = new Person()
                {
                    FirstName = NewPersonFirstName,
                    LastName = NewPersonLastName,
                    MiddleName = NewPersonMiddleName,
                    DateOfBirth = NewPersonDateOfBirth,
                    Gender = NewPersonGender
                };
                _personService.AddPersonAsync(newPerson);
                RefreshPersons();
            }
        });

        public ICommand EditPersonCommand => new RelayCommand<Person>(person =>
        {
            var editViewModel = new EditPersonViewModel(person);
            var editWindow = new EditPersonWindow { DataContext = editViewModel };

            editViewModel.OnConfirm += async updatedPerson =>
            {
                await _personService.UpdatePersonAsync(updatedPerson);
                RefreshPersons();
                editWindow.Close();
            };

            editWindow.ShowDialog();
        });

        public ICommand DeletePersonCommand => new RelayCommand<Person>(person =>
        {
            if (person != null)
            {
                _personService.DeletePersonAsync(person.Id);
                RefreshPersons();
            }
        });

        private async void RefreshPersons()
        {
            var personsList = await _personService.GetAllPeopleAsync();
            Persons.Clear();
            foreach (var person in personsList)
            {
                Persons.Add(person);
            }
        }
    }
}
