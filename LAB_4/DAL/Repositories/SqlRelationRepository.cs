using LAB_4.DAL.Models;
using LAB_4.DAL.Interfaces;
using LAB_4.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL.Repositories;

public class SqlRelationRepository : IRelationRepository
{
    private readonly ApplicationDbContext _context;

    public SqlRelationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Relation>> GetAllAsync()
    {
        return await _context.Relations.ToListAsync();
    }

    public async Task<Relation?> GetByIdAsync(int id)
    {
        return await _context.Relations.FindAsync(id);
    }

    public async Task AddAsync(Relation relation)
    {
        await _context.Relations.AddAsync(relation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Relation relation)
    {
        _context.Relations.Update(relation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var relation = await GetByIdAsync(id);
        if (relation != null)
        {
            _context.Relations.Remove(relation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Relation>> GetRelationsByPersonIdAsync(int personId)
    {
        return await _context.Relations
            .Where(r => r.PersonId == personId || r.RelatedPersonId == personId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Relation>> GetRelationsByPersonIdAndTypeAsync(int personId, RelationType relationType)
    {
        return await _context.Relations
            .Where(r => (r.PersonId == personId || r.RelatedPersonId == personId) && r.RelationType == relationType)
            .ToListAsync();
    }

    public async Task<Relation?> GetRelationBetweenPeopleAsync(int person1Id, int person2Id)
    {
        return await _context.Relations
            .FirstOrDefaultAsync(r =>
                (r.PersonId == person1Id && r.RelatedPersonId == person2Id) ||
                (r.PersonId == person2Id && r.RelatedPersonId == person1Id));
    }

    public async Task DeleteRelationsByPersonIdAsync(int personId)
    {
        var relations = await GetRelationsByPersonIdAsync(personId);
        _context.Relations.RemoveRange(relations);
        await _context.SaveChangesAsync();
    }
}
