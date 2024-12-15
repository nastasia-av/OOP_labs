using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.DAL.Interfaces
{
    public interface IRelationRepository
    {
        Task AddAsync(Relation relation);
        Task DeleteAsync(int relationId);
        Task<IEnumerable<Relation>> GetRelationsByPersonIdAsync(int personId);
        Task<IEnumerable<Relation>> GetRelationsByPersonIdAsync(int personId, RelationType relationType);
        Task<IEnumerable<Relation>> GetRelationsForPersonAsync(int personId);
        Task<IEnumerable<Relation>> GetRelationsForPersonAsync(int personId, RelationType relationType);
        Task<IEnumerable<Relation>> GetAllAsync();
        Task<List<Relation>> GetRelationsForPeopleAsync(List<int> personIds);

    }
}
