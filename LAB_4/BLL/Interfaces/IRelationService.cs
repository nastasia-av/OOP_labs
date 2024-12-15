using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.BLL.Interfaces
{
    public interface IRelationService
    {
        event Action RelationUpdated;
        Task AddRelationAsync(int person1Id, int person2Id, RelationType relationType); 
        Task RemoveRelationAsync(int relationId); 
        Task<IEnumerable<Relation>> GetRelationsForPersonAsync(int personId);
        Task<int> GetAncestorAgeAtBirthAsync(int ancestorId, int descendantId);
        Task<IEnumerable<Person>> FindCommonAncestorsAsync(int person1Id, int person2Id);
        Task<IEnumerable<Person>> GetAncestorsAsync(int personId);
        Task<IEnumerable<Person>> GetDescendantsAsync(int personId);
        Task<IEnumerable<Relation>> GetAllRelationsAsync();
        Task<List<Relation>> GetRelationsForPeopleAsync(List<int> personIds);
    }
}
