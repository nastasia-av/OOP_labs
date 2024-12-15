using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.DAL.Interfaces
{
    public interface IRelationRepository
    {
        Task<IEnumerable<Relation>> GetAllAsync();
        Task<Relation?> GetByIdAsync(int id);
        Task AddAsync(Relation relation);
        Task UpdateAsync(Relation relation);
        Task DeleteAsync(int id);
        Task<IEnumerable<Relation>> GetRelationsByPersonIdAsync(int personId);
        Task<IEnumerable<Relation>> GetRelationsByPersonIdAndTypeAsync(int personId, RelationType relationType);
        Task<Relation?> GetRelationBetweenPeopleAsync(int person1Id, int person2Id);
        Task DeleteRelationsByPersonIdAsync(int personId);

    }
}
