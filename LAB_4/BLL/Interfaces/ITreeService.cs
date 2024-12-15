using LAB_4.DAL.Models;

namespace LAB_4.BLL.Interfaces
{
    public interface ITreeService
    {
        Task<Tree?> GetActiveTreeAsync(); 
        Task CreateNewTreeAsync(); 
        Task AddPersonToTreeAsync(int personId); 
        Task RemovePersonFromTreeAsync(int personId); 
        Task<IEnumerable<Person>> GetAllPeopleInTreeAsync();
        Task<List<Person>> GetMainTreePeopleAsync();
    }
}
