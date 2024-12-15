using LAB_4.BLL.Interfaces;
using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;
using LAB_4.DAL.Interfaces;
using LAB_4.BLL.Extensions;

namespace LAB_4.BLL.Services
{
    public class RelationService : IRelationService
    {
        private readonly IRelationRepository _relationRepository;
        private readonly IPersonRepository _personRepository;

        public event Action? RelationUpdated;

        public RelationService(IRelationRepository relationRepository, IPersonRepository personRepository)
        {
            _relationRepository = relationRepository;
            _personRepository = personRepository;
        }

        public async Task AddRelationAsync(int person1Id, int person2Id, RelationType relationType)
        {
            var existingRelations = await _relationRepository.GetRelationsForPersonAsync(person1Id);

            bool isRelationExist = existingRelations.Any(r => r.RelatedPersonId == person2Id && r.RelationType == relationType);
            if (isRelationExist)
            {
                return;
            }

            var relation = new Relation
            {
                PersonId = person1Id,
                RelatedPersonId = person2Id,
                RelationType = relationType
            };

            await _relationRepository.AddAsync(relation);
            RelationUpdated?.Invoke();

            var reverseRelation = GetReverseRelation(relationType);
            if (reverseRelation.HasValue)
            {
                await _relationRepository.AddAsync(new Relation
                {
                    PersonId = person2Id,
                    RelatedPersonId = person1Id,
                    RelationType = reverseRelation.Value
                });
            }
            RelationUpdated?.Invoke();
        }

        public async Task RemoveRelationAsync(int relationId)
        {
            await _relationRepository.DeleteAsync(relationId);
            RelationUpdated?.Invoke();
        }

        public async Task<IEnumerable<Relation>> GetRelationsForPersonAsync(int personId)
        {
            return await _relationRepository.GetRelationsByPersonIdAsync(personId);
        }


        private RelationType? GetReverseRelation(RelationType relationType)
        {
            return relationType switch
            {
                RelationType.Parent => RelationType.Child,
                RelationType.Child => RelationType.Parent,
                RelationType.Sibling => RelationType.Sibling,
                RelationType.Spouse => RelationType.Spouse,
                RelationType.Grandparent => RelationType.Grandchild,
                RelationType.Grandchild => RelationType.Grandparent,
                RelationType.AuntUncle => RelationType.NephewNiece,
                RelationType.NephewNiece => RelationType.AuntUncle,
                RelationType.Cousin => RelationType.Cousin,
                RelationType.ParentInLaw => RelationType.ChildInLaw,
                RelationType.ChildInLaw => RelationType.ParentInLaw,
                _ => null
            };
        }


        public async Task<int> GetAncestorAgeAtBirthAsync(int ancestorId, int descendantId)
        {
            var ancestor = await _personRepository.GetByIdAsync(ancestorId);
            var descendant = await _personRepository.GetByIdAsync(descendantId);

            if (ancestor == null || descendant == null)
                throw new ArgumentException("Один из указанных людей не найден.");

            var ancestorBirthDate = ancestor.DateOfBirth;
            var descendantBirthDate = descendant.DateOfBirth;

            var ageAtBirth = descendantBirthDate - ancestorBirthDate;

            return (int)(ageAtBirth.Days / 365);
        }

        public async Task<IEnumerable<Person>> FindCommonAncestorsAsync(int person1Id, int person2Id)
        {
            var ancestors1 = await GetAncestorsAsync(person1Id);
            var ancestors2 = await GetAncestorsAsync(person2Id);

            return ancestors1.Intersect(ancestors2, new PersonEqualityComparer());
        }

        public async Task<IEnumerable<Person>> GetAncestorsAsync(int personId)
        {
            var relations = await _relationRepository.GetAllAsync();
            var persons = await _personRepository.GetAllAsync();

            var result = new List<Person>();
            var stack = new Stack<int>();
            stack.Push(personId);

            while (stack.Count > 0)
            {
                var currentId = stack.Pop();
                var parents = relations
                    .Where(r => r.RelatedPersonId == currentId && r.RelationType == RelationType.Parent)
                    .Select(r => r.PersonId)
                    .ToList();

                foreach (var parentId in parents)
                {
                    if (result.All(p => p.Id != parentId))
                    {
                        var parent = persons.FirstOrDefault(p => p.Id == parentId);
                        if (parent != null)
                        {
                            result.Add(parent);
                            stack.Push(parentId);
                        }
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<Person>> GetDescendantsAsync(int personId)
        {
            var relations = await _relationRepository.GetAllAsync();
            var persons = await _personRepository.GetAllAsync();

            var result = new List<Person>();
            var stack = new Stack<int>();
            stack.Push(personId);

            while (stack.Count > 0)
            {
                var currentId = stack.Pop();
                var children = relations
                    .Where(r => r.PersonId == currentId && r.RelationType == RelationType.Parent)
                    .Select(r => r.RelatedPersonId)
                    .ToList();

                foreach (var childId in children)
                {
                    if (result.All(p => p.Id != childId))
                    {
                        var child = persons.FirstOrDefault(p => p.Id == childId);
                        if (child != null)
                        {
                            result.Add(child);
                            stack.Push(childId);
                        }
                    }
                }
            }

            return result;
        }


        public async Task<IEnumerable<Relation>> GetAllRelationsAsync()
        {
            return await _relationRepository.GetAllAsync();
        }

        public async Task<List<Relation>> GetRelationsForPeopleAsync(List<int> personIds)
        {
            return await _relationRepository.GetRelationsForPeopleAsync(personIds);
        }

    }
}
