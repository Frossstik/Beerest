using Beerest.Models;
using Microsoft.EntityFrameworkCore;

namespace Beerest
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Bars> bars { get; set; }
        public DbSet<Beers> beers { get; set; }
        public DbSet<Persons> persons { get; set; }
    }
}
