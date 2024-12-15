using LAB_4.DAL.Models;
using LAB_4.DAL.Models.Enums;
using LAB_4.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL.Repositories
{
    public class SqlRelationRepository : IRelationRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlRelationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Relation relation)
        {
            await _context.Relations.AddAsync(relation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int relationId)
        {
            var relation = await _context.Relations.FindAsync(relationId);
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

        public async Task<IEnumerable<Relation>> GetRelationsByPersonIdAsync(int personId, RelationType relationType)
        {
            return await _context.Relations
                .Where(r => (r.PersonId == personId || r.RelatedPersonId == personId) && r.RelationType == relationType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Relation>> GetRelationsForPersonAsync(int personId)
        {
            return await _context.Relations
                .Where(r => r.PersonId == personId || r.RelatedPersonId == personId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Relation>> GetRelationsForPersonAsync(int personId, RelationType relationType)
        {
            return await _context.Set<Relation>()
                                 .Where(r => (r.PersonId == personId || r.RelatedPersonId == personId) && r.RelationType == relationType)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Relation>> GetAllAsync()
        {
            return await _context.Relations.ToListAsync();
        }

        public async Task<List<Relation>> GetRelationsForPeopleAsync(List<int> personIds)
        {
            return await _context.Relations
                .Where(r => personIds.Contains(r.PersonId) || personIds.Contains(r.RelatedPersonId))
                .ToListAsync();
        }

    }
}
