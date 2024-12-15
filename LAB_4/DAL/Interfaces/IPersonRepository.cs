using LAB_4.DAL.Models;

namespace LAB_4.DAL.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(int id);
        Task<IEnumerable<Person>> GetByFullNameAsync(string firstName, string lastName, string middleName);

    }
}
