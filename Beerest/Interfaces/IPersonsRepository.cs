using Beerest.Models;

namespace Beerest.Interfaces
{
    public interface IPersonsRepository : ICrudRepository<Persons>
    {
        Task<IEnumerable<Persons>> GetAllAsync();
        Task<Persons> GetByIdAsync(int id);
    }
}
