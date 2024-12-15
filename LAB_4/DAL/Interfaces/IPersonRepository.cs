using LAB_4.DAL.Models;

namespace LAB_4.DAL.Interfaces
{
    public interface IPersonRepository
    {
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(int personId);
        Task<Person?> GetByIdAsync(int personId);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<List<Person>> GetMainTreePeopleAsync();
    }
}
