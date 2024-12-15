using LAB_4.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL.Interfaces
{
    public interface ITreeRepository
    {
        Task<Tree?> GetActiveTreeAsync();
        Task CreateNewTreeAsync();
        Task DeleteTreeAsync();
        Task AddPersonToTreeAsync(int personId);
        Task RemovePersonFromTreeAsync(int personId);
        Task<IEnumerable<Person>> GetAllPeopleInTreeAsync();
    }
}
