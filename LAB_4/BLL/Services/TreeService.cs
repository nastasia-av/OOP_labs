using LAB_4.BLL.Interfaces;
using LAB_4.DAL.Models;
using LAB_4.DAL.Interfaces;

namespace LAB_4.BLL.Services
{
    public class TreeService : ITreeService
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IPersonRepository _personRepository;

        public TreeService(ITreeRepository treeRepository, IPersonRepository personRepository)
        {
            _treeRepository = treeRepository;
            _personRepository = personRepository;
        }

        public async Task<Tree?> GetActiveTreeAsync()
        {
            return await _treeRepository.GetActiveTreeAsync();
        }

        public async Task CreateNewTreeAsync()
        {
            var allPeople = await _personRepository.GetAllAsync();
            foreach (var person in allPeople)
            {
                person.IsInMainTree = false;
                await _personRepository.UpdateAsync(person);
            }
            
            await _treeRepository.CreateNewTreeAsync();
        }

        public async Task AddPersonToTreeAsync(int personId)
        {
            var activeTree = await _treeRepository.GetActiveTreeAsync();
            if (activeTree == null)
            {
                await CreateNewTreeAsync();
            }
            await _treeRepository.AddPersonToTreeAsync(personId);
        }

        public async Task RemovePersonFromTreeAsync(int personId)
        {
            var activeTree = await _treeRepository.GetActiveTreeAsync();
            if (activeTree == null)
            {
                throw new InvalidOperationException("Активное дерево отсутствует.");
            }
            await _treeRepository.RemovePersonFromTreeAsync(personId);
        }

        public async Task<IEnumerable<Person>> GetAllPeopleInTreeAsync()
        {
            var activeTree = await _treeRepository.GetActiveTreeAsync();
            return activeTree?.Persons ?? Enumerable.Empty<Person>();
        }

        public async Task<List<Person>> GetMainTreePeopleAsync()
        {
            return await _personRepository.GetMainTreePeopleAsync();
        }

    }
}
