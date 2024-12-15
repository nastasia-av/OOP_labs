using LAB_4.DAL.Models;

namespace LAB_4.BLL.Interfaces
{
    public interface IPersonService
    {
        event Action PersonUpdated;
        Task AddPersonAsync(Person person); 
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int personId); 
        Task<Person?> GetPersonByIdAsync(int personId); 
        Task<IEnumerable<Person>> GetAllPeopleAsync();
    }
}
