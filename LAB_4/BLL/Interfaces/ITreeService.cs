using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;

namespace LAB_4.BLL.Interfaces
{
    public interface ITreeService
    {
        Task CreateNewTreeAsync();
        Task AddPersonToTreeAsync(int personId);
        Task RemovePersonFromTreeAsync(int personId);
        Task AddRelationAsync(int personId1, int personId2, RelationType relationType);
        Task<IEnumerable<Person>> GetRelativesAsync(int personId, RelationType relationType);
        Task<IEnumerable<Person>> GetCommonRelativesAsync(int personId1, int personId2);
        Task<int?> CalculateAncestorAgeAtBirthAsync(int ancestorId, int descendantId);
    }
}
