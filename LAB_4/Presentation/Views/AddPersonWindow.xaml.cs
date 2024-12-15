using System;
using System.Windows.Input;
using LAB_4.Presentation.Utils;
using LAB_4.BLL.Services;

namespace LAB_4.Presentation.ViewModels
{
    public class AddPersonViewModel
    {
        private readonly GenealogyTreeService _service;

        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string Gender { get; set; }

        public ICommand AddPersonCommand { get; }

        public AddPersonViewModel(GenealogyTreeService service)
        {
            _service = service;
            AddPersonCommand = new RelayCommand(AddPerson);
        }

        private void AddPerson()
        {
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Gender))
                return; 

            _service.AddPerson(FullName, DateOfBirth, Gender);
        }
    }
}
