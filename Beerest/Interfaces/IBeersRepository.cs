using Beerest.Models;

namespace Beerest.Interfaces
{
    public interface IBeersRepository : ICrudRepository<Beers>
    {
        Task<IEnumerable<Beers>> GetAllAsync();
        Task<Beers> GetByIdAsync(int id);
    }
}
