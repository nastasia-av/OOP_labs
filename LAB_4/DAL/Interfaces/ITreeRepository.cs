using LAB_4.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace LAB_4.DAL.Interfaces
{
    public interface ITreeRepository
    {
        Task<Tree?> GetCurrentTreeAsync();
        Task ResetTreeAsync();

    }
}
