using LAB_4.DAL.Interfaces;
using LAB_4.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL.Repositories
{
    public class SqlTreeRepository : ITreeRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlTreeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tree?> GetActiveTreeAsync()
        {
            return await _context.Trees
                .Include(t => t.Persons) 
                .FirstOrDefaultAsync();
        }

        public async Task CreateNewTreeAsync()
        {
            var existingTree = await _context.Trees.FirstOrDefaultAsync();
            if (existingTree != null)
            {
                _context.Trees.Remove(existingTree);
                await _context.SaveChangesAsync();
            }

            var newTree = new Tree
            {
                Persons = new List<Person>() 
            };

            _context.Trees.Add(newTree);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTreeAsync()
        {
            var tree = await _context.Trees.FirstOrDefaultAsync();
            if (tree != null)
            {
                _context.Trees.Remove(tree);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPersonToTreeAsync(int personId)
        {
            var tree = await _context.Trees
                .Include(t => t.Persons)
                .FirstOrDefaultAsync();

            if (tree != null)
            {
                var person = await _context.People
                    .FirstOrDefaultAsync(p => p.Id == personId);

                if (person != null)
                {
                    person.IsInMainTree = true;
                    tree.Persons.Add(person);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemovePersonFromTreeAsync(int personId)
        {
            var tree = await _context.Trees
                .Include(t => t.Persons)
                .FirstOrDefaultAsync();

            if (tree != null)
            {
                var person = tree.Persons.FirstOrDefault(p => p.Id == personId);
                if (person != null)
                {
                    person.IsInMainTree = false ;
                    tree.Persons.Remove(person);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<Person>> GetAllPeopleInTreeAsync()
        {
            var tree = await _context.Trees
                .Include(t => t.Persons)
                .FirstOrDefaultAsync();

            return tree?.Persons ?? Enumerable.Empty<Person>();
        }
    }
}
