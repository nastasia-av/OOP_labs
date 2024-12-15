using LAB_4.DAL.Models;
using LAB_4.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL.Repositories;

public class SqlTreeRepository : ITreeRepository
{
    private readonly ApplicationDbContext _context;

    public SqlTreeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Tree> GetCurrentTreeAsync()
    {
        var tree = await _context.Trees.FirstOrDefaultAsync();
        if (tree == null)
        {
            tree = new Tree();
            _context.Trees.Add(tree);
            await _context.SaveChangesAsync();
        }
        return tree;
    }

    public async Task ResetTreeAsync()
    {
        var tree = await _context.Trees.FirstOrDefaultAsync();
        if (tree != null)
        {
            _context.Trees.Remove(tree);
            await _context.SaveChangesAsync();
        }
        var newTree = new Tree();
        _context.Trees.Add(newTree);
        await _context.SaveChangesAsync();
    }
}
