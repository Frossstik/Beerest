using Beerest.Interfaces;
using Beerest.Models;
using Microsoft.EntityFrameworkCore;

namespace Beerest.Repositories
{
    public class BeersRepository : AppRepository<Beers>, IBeersRepository
    {
        public BeersRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Beers>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Beers?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
