using LAB_4.BLL.Interfaces;
using LAB_4.DAL.Models;
using LAB_4.DAL.Interfaces;

namespace LAB_4.BLL.Services;

public class TreeService : ITreeService
{
    private readonly ITreeRepository _treeRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IRelationRepository _relationRepository;

    public TreeService(ITreeRepository treeRepository, IPersonRepository personRepository, IRelationRepository relationRepository)
    {
        _treeRepository = treeRepository;
        _personRepository = personRepository;
        _relationRepository = relationRepository;
    }

    // Получение текущего дерева
    public async Task<Tree?> GetActiveTreeAsync()
    {
        return await _treeRepository.GetActiveTreeAsync();
    }

    // Создание нового дерева
    public async Task CreateNewTreeAsync()
    {
        var newTree = new Tree();
        await _treeRepository.CreateNewTreeAsync(newTree);
    }

    // Добавление человека в дерево
    public async Task AddPersonToTreeAsync(int personId)
    {
        var activeTree = await _treeRepository.GetActiveTreeAsync();
        if (activeTree == null)
        {
            throw new InvalidOperationException("Активное дерево отсутствует.");
        }

        await _treeRepository.AddPersonToTreeAsync(activeTree.Id, personId);
    }

    // Удаление человека из дерева
    public async Task RemovePersonFromTreeAsync(int personId)
    {
        var activeTree = await _treeRepository.GetActiveTreeAsync();
        if (activeTree == null)
        {
            throw new InvalidOperationException("Активное дерево отсутствует.");
        }

        await _treeRepository.RemovePersonFromTreeAsync(activeTree.Id, personId);
    }

    // Установление связи между двумя людьми
    public async Task AddRelationAsync(int person1Id, int person2Id, RelationType relationType)
    {
        var person1 = await _personRepository.GetByIdAsync(person1Id);
        var person2 = await _personRepository.GetByIdAsync(person2Id);

        if (person1 == null || person2 == null)
        {
            throw new ArgumentException("Один из указанных людей не существует.");
        }

        // Проверка на дублирование связи
        var existingRelations = await _relationRepository.GetRelationsByPersonIdAsync(person1Id);
        if (existingRelations.Any(r => r.Person2Id == person2Id && r.Type == relationType))
        {
            throw new InvalidOperationException("Такая связь уже существует.");
        }

        // Добавление основной связи
        var relation = new Relation
        {
            Person1Id = person1Id,
            Person2Id = person2Id,
            Type = relationType
        };

        await _relationRepository.AddAsync(relation);

        // Автоматическое добавление обратной связи, если необходимо
        var reverseRelation = RelationTypeExtensions.GetReverseRelation(relationType);
        if (reverseRelation != null)
        {
            var reverse = new Relation
            {
                Person1Id = person2Id,
                Person2Id = person1Id,
                Type = reverseRelation.Value
            };

            await _relationRepository.AddAsync(reverse);
        }

        // Динамическое добавление других связей
        await AddDynamicRelationsAsync(person1Id, person2Id, relationType);
    }

    // Удаление связи
    public async Task RemoveRelationAsync(int relationId)
    {
        await _relationRepository.DeleteAsync(relationId);
    }

    // Вывод всех людей в дереве
    public async Task<IEnumerable<Person>> GetAllPeopleInTreeAsync()
    {
        var activeTree = await _treeRepository.GetActiveTreeAsync();
        return activeTree?.Persons ?? Enumerable.Empty<Person>();
    }

    // Получение ближайших родственников (родители, дети)
    public async Task<IEnumerable<Person>> GetCloseRelativesAsync(int personId)
    {
        var relations = await _relationRepository.GetRelationsByPersonIdAsync(personId);
        var closeRelations = relations.Where(r =>
            r.Type == RelationType.Parent ||
            r.Type == RelationType.Child);

        var personIds = closeRelations.Select(r => r.Person1Id == personId ? r.Person2Id : r.Person1Id);
        return await _personRepository.GetAllAsync().ContinueWith(t => t.Result.Where(p => personIds.Contains(p.Id)));
    }

    // Автоматическое добавление всех возможных связей
    private async Task AddDynamicRelationsAsync(int person1Id, int person2Id, RelationType relationType)
    {
        var activeTree = await _treeRepository.GetActiveTreeAsync();
        if (activeTree == null)
        {
            throw new InvalidOperationException("Активное дерево отсутствует.");
        }

        // Получение всех людей в дереве
        var allPeople = activeTree.Persons.ToList();

        // Логика динамического добавления связей
        foreach (var person in allPeople)
        {
            if (person.Id == person1Id || person.Id == person2Id)
                continue;

            // Пример: Если добавлена связь "родитель", то нужно добавить "дедушка/бабушка"
            if (relationType == RelationType.Parent)
            {
                var childRelations = await _relationRepository.GetRelationsByPersonIdAsync(person1Id);
                foreach (var childRelation in childRelations)
                {
                    if (childRelation.Type == RelationType.Child)
                    {
                        var grandChildId = childRelation.Person2Id;
                        var grandParentRelation = new Relation
                        {
                            Person1Id = person2Id,
                            Person2Id = grandChildId,
                            Type = RelationType.GrandParent
                        };

                        await _relationRepository.AddAsync(grandParentRelation);
                    }
                }
            }
        }
    }
}
