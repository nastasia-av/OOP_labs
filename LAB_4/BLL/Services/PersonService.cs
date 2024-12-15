using LAB_4.BLL.Interfaces;
using LAB_4.DAL.Models;
using LAB_4.DAL.Interfaces;

namespace LAB_4.BLL.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public event Action? PersonUpdated;
    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task AddPersonAsync(Person person)
    {
        await _personRepository.AddAsync(person);
        PersonUpdated?.Invoke();
    }

    public async Task UpdatePersonAsync(Person person)
    {
        await _personRepository.UpdateAsync(person);
        PersonUpdated?.Invoke();
    }

    public async Task DeletePersonAsync(int personId)
    {
        await _personRepository.DeleteAsync(personId);
        PersonUpdated?.Invoke();
    }

    public async Task<Person?> GetPersonByIdAsync(int personId)
    {
        return await _personRepository.GetByIdAsync(personId);
    }

    public async Task<IEnumerable<Person>> GetAllPeopleAsync()
    {
        return await _personRepository.GetAllAsync();
    }
}
