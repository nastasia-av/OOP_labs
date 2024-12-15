using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_4.BLL.Interfaces;
using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;
using LAB_4.Presentation.Views;
using LAB_4.Presentation.Utils;

namespace LAB_4.Presentation.ViewModels
{
    public class RelationManagementViewModel
    {
        private readonly IRelationService _relationService;
        private readonly IPersonService _personService;

        public ObservableCollection<Relation> Relations { get; private set; }
        public ObservableCollection<Person> People { get; private set; }

        public RelationManagementViewModel(IRelationService relationService, IPersonService personService)
        {
            _relationService = relationService;
            _personService = personService;

            Relations = new ObservableCollection<Relation>();
            People = new ObservableCollection<Person>();

            _ = RefreshPeople();
            RefreshRelations();
        }

        public Person SelectedPerson { get; set; }
        public Person RelatedPerson { get; set; }
        public RelationType SelectedRelationType { get; set; }

        public Person PersonForSearch { get; set; }

        public ObservableCollection<RelationType> RelationTypes { get; set; } = new ObservableCollection<RelationType>((RelationType[])Enum.GetValues(typeof(RelationType)));
        public ICommand AddRelationCommand => new RelayCommand(() =>
        {
            if (SelectedPerson != null && RelatedPerson != null && SelectedPerson != RelatedPerson)
            {
                _relationService.AddRelationAsync(SelectedPerson.Id, RelatedPerson.Id, SelectedRelationType);
                RefreshRelations();
            }
        });

        public ICommand DeleteRelationCommand => new RelayCommand<Relation>(relation =>
        {
            if (relation != null)
            {
                _relationService.RemoveRelationAsync(relation.Id);
                RefreshRelations();
            }
        });

        public ICommand FindRelationsCommand => new AsyncRelayCommand(async () =>
        {
            if (PersonForSearch != null)
            {
                var relationsList = await _relationService.GetRelationsForPersonAsync(PersonForSearch.Id);
                Relations.Clear();
                foreach (var relation in relationsList)
                {
                    Relations.Add(relation);
                }
            }
        });

        public ICommand ShowAllRelationsCommand => new RelayCommand(() => RefreshRelations());

        private async Task RefreshPeople()
        {
            var peopleList = await _personService.GetAllPeopleAsync();
            People.Clear();
            foreach (var person in peopleList)
            {
                People.Add(person);
            }
        }

        private async void RefreshRelations()
        {
            var relationsList = await _relationService.GetAllRelationsAsync();
            Relations.Clear();
            foreach (var relation in relationsList)
            {
                Relations.Add(relation);
            }
        }

        public string GetLocalizedRelationType(RelationType relationType)
        {
            return relationType switch
            {
                RelationType.Parent => "Родитель",
                RelationType.Child => "Ребенок",
                RelationType.Sibling => "Брат/Сестра",
                RelationType.Spouse => "Супруг/Супруга",
                RelationType.Grandparent => "Дедушка/Бабушка",
                RelationType.Grandchild => "Внук/Внучка",
                RelationType.AuntUncle => "Тетя/Дядя",
                RelationType.NephewNiece => "Племянник/Племянница",
                RelationType.Cousin => "Кузен/Кузина",
                RelationType.ParentInLaw => "Тесть/Теща",
                RelationType.ChildInLaw => "Зять/Невестка",
                RelationType.SiblingInLaw => "Свояк/Свояченица",
                _ => "Неизвестно"
            };
        }
    }
}
