using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;
using LAB_4.Presentation.Utils;

namespace LAB_4.Presentation.ViewModels
{
    public class EditPersonViewModel : INotifyPropertyChanged
    {
        private string _personFirstName;
        private string _personLastName;
        private string _personMiddleName;
        private DateTime _personDateOfBirth;
        private Gender _personGender;

        public ObservableCollection<Gender> Genders { get; set; } = new ObservableCollection<Gender>((Gender[])Enum.GetValues(typeof(Gender)));
        public string PersonFirstName
        {
            get => _personFirstName;
            set
            {
                if (_personFirstName != value)
                {
                    _personFirstName = value;
                    OnPropertyChanged(nameof(PersonFirstName));
                }
            }
        }

        public string PersonLastName
        {
            get => _personLastName;
            set
            {
                if (_personLastName != value)
                {
                    _personLastName = value;
                    OnPropertyChanged(nameof(PersonLastName));
                }
            }
        }

        public string PersonMiddleName
        {
            get => _personMiddleName;
            set
            {
                if (_personMiddleName != value)
                {
                    _personMiddleName = value;
                    OnPropertyChanged(nameof(PersonMiddleName));
                }
            }
        }

        public DateTime PersonDateOfBirth
        {
            get => _personDateOfBirth;
            set
            {
                if (_personDateOfBirth != value)
                {
                    _personDateOfBirth = value;
                    OnPropertyChanged(nameof(PersonDateOfBirth));
                }
            }
        }

        public Gender PersonGender
        {
            get => _personGender;
            set
            {
                if (_personGender != value)
                {
                    _personGender = value;
                    OnPropertyChanged(nameof(PersonGender));
                }
            }
        }

        public ICommand ConfirmCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<Person>? OnConfirm;

        public EditPersonViewModel(Person person)
        {
            PersonFirstName = person.FirstName;
            PersonLastName = person.LastName;
            PersonMiddleName = person.MiddleName;
            PersonDateOfBirth = person.DateOfBirth;
            PersonGender = person.Gender;

            ConfirmCommand = new RelayCommand(() =>
            {
                OnConfirm?.Invoke(new Person
                {
                    FirstName = PersonFirstName,
                    LastName = PersonLastName,
                    MiddleName = PersonMiddleName,
                    DateOfBirth = PersonDateOfBirth,
                    Gender = PersonGender
                });
            });
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
