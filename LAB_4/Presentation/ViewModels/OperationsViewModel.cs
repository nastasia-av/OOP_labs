using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LAB_4.BLL.Interfaces;
using LAB_4.DAL.Models;
using LAB_4.Presentation.Utils;

namespace LAB_4.Presentation.ViewModels
{
    public class OperationsViewModel
    {
        private readonly IRelationService _relationService;
        private readonly IPersonService _personService;

        public ObservableCollection<Person> People { get; private set; }
        public ObservableCollection<Result> Results { get; private set; }

        public OperationsViewModel(IRelationService relationService, IPersonService personService)
        {
            _relationService = relationService;
            _personService = personService;

            People = new ObservableCollection<Person>();
            Results = new ObservableCollection<Result>();

            _personService.PersonUpdated += async () => await RefreshPeople();
            _ = RefreshPeople();
        }

        public Person SelectedPerson { get; set; }
        public Person SelectedAncestor { get; set; }
        public Person SelectedDescendant { get; set; }
        public Person SelectedPerson1 { get; set; }
        public Person SelectedPerson2 { get; set; }
        public string GetFullName(Person person)
        {
            return $"{person.LastName} {person.FirstName} {person.MiddleName}".Trim();
        }
        public ICommand FindCloseRelativesCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedPerson != null)
            {
                var parents = await _relationService.GetAncestorsAsync(SelectedPerson.Id);
                var children = await _relationService.GetDescendantsAsync(SelectedPerson.Id);

                if (!parents.Any() && !children.Any())
                {
                    Results.Add(new Result { Description = $"Нет ближайших родственников для {GetFullName(SelectedPerson)}" });
                    return;
                }

                Results.Add(new Result { Description = $"Ближайшие родственники {GetFullName(SelectedPerson)}:" });

                foreach (var parent in parents)
                {
                    Results.Add(new Result { Description = $"{GetFullName(parent)} - Родитель" });
                }

                foreach (var child in children)
                {
                    Results.Add(new Result { Description = $"{GetFullName(child)} - Ребенок" });
                }
            }
        });


        public ICommand CalculateAncestorAgeCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedAncestor != null && SelectedDescendant != null)
            {
                var descendants = await _relationService.GetDescendantsAsync(SelectedAncestor.Id);

                if (!descendants.Any(d => d.Id == SelectedDescendant.Id))
                {
                    Results.Add(new Result
                    {
                        Description = $"{GetFullName(SelectedDescendant)} не является потомком {GetFullName(SelectedAncestor)}"
                    });
                    return;
                }

                var ageAtBirth = await _relationService.GetAncestorAgeAtBirthAsync(SelectedAncestor.Id, SelectedDescendant.Id);
                Results.Add(new Result
                {
                    Description = $"При рождении {GetFullName(SelectedDescendant)} возраст {GetFullName(SelectedAncestor)} был {ageAtBirth} лет"
                });
            }
        });


        public ICommand FindCommonAncestorsCommand => new AsyncRelayCommand(async () =>
        {
            if (SelectedPerson1 != null && SelectedPerson2 != null)
            {
                var commonAncestors = await _relationService.FindCommonAncestorsAsync(SelectedPerson1.Id, SelectedPerson2.Id);

                if (commonAncestors.Any())
                {
                    Results.Add(new Result { Description = "Общие предки:" });
                    foreach (var ancestor in commonAncestors)
                    {
                        Results.Add(new Result { Description = GetFullName(ancestor) });
                    }
                }
                else
                {
                    Results.Add(new Result { Description = "Нет общих предков." });
                }
            }
        });

        public ICommand ClearResultsCommand => new RelayCommand(() => Results.Clear());

        private async Task RefreshPeople()
        {
            var peopleList = await _personService.GetAllPeopleAsync();
            People.Clear();
            foreach (var person in peopleList)
            {
                People.Add(person);
            }
        }
    }

    public class Result
    {
        public string Description { get; set; }
    }
}
