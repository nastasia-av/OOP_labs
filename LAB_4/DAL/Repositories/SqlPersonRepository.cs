using LAB_4.DAL.Models;
using Microsoft.EntityFrameworkCore;
using LAB_4.DAL.Interfaces;

namespace LAB_4.DAL.Repositories;

public class SqlPersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _context;

    public SqlPersonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _context.People.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.People.FindAsync(id);
    }

    public async Task AddAsync(Person person)
    {
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var person = await GetByIdAsync(id);
        if (person != null)
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Person>> GetByFullNameAsync(string firstName, string lastName, string middleName)
    {
        return await _context.People
            .Where(p => p.FirstName == firstName && p.LastName == lastName && p.MiddleName == middleName)
            .ToListAsync();
    }

}
