using Beerest.Interfaces;
using Beerest.Models;
using Microsoft.EntityFrameworkCore;

namespace Beerest.Repositories
{
    public class PersonsRepository : AppRepository<Persons>, IPersonsRepository
    {
        public PersonsRepository(AppDbContext context) : base(context)
        {
            Console.WriteLine($"BeersRepository создан: {this.GetHashCode()}" );
        }

        public async Task<IEnumerable<Persons>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.Bar).ThenInclude(b => b.Beers).ToListAsync();
        }

        public async Task<Persons?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(p => p.Bar).ThenInclude(b => b.Beers).FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
