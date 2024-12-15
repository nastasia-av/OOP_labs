using System.Collections.ObjectModel;
using System.Windows.Input;
using GenealogyTree.UI.Utils;
using LAB_4.BLL.Services;

namespace GenealogyTree.UI.ViewModels
{
    public class SetRelationshipViewModel
    {
        private readonly GenealogyTreeService _service;

        public ObservableCollection<Person> People { get; }
        public Person SelectedPerson { get; set; }
        public Person SelectedRelative { get; set; }
        public string RelationType { get; set; }

        public ICommand SetRelationshipCommand { get; }

        public SetRelationshipViewModel(GenealogyTreeService service)
        {
            _service = service;
            People = new ObservableCollection<Person>(_service.GetAllPeople());
            SetRelationshipCommand = new RelayCommand(SetRelationship);
        }

        private void SetRelationship()
        {
            if (SelectedPerson == null || SelectedRelative == null || string.IsNullOrWhiteSpace(RelationType))
                return;

            _service.AddRelationship(SelectedPerson.Id, SelectedRelative.Id, RelationType);
        }
    }
}
