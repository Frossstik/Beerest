using Beerest.Models;

namespace Beerest.Interfaces
{
    public interface IBarsRepository : ICrudRepository<Bars>
    {
        Task<IEnumerable<Bars>> GetAllAsync();
        Task<Bars> GetByIdAsync(int id);
    }
}
