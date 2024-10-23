using Beerest.Interfaces;
using Beerest.Models;
using Microsoft.EntityFrameworkCore;

namespace Beerest.Repositories
{
    public class BarsRepository : AppRepository<Bars>, IBarsRepository
    {
        public BarsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Bars>> GetAllAsync()
        {
            return await _dbSet.Include(b => b.Beers).ToListAsync();
        }

        public async Task<Bars?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(b => b.Beers).FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}

