using Beerest.Models;
using Microsoft.EntityFrameworkCore;

namespace Beerest
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Console.WriteLine("AppDbContext создан: " + this.GetHashCode());
        }

        public DbSet<Bars> bars { get; set; }
        public DbSet<Beers> beers { get; set; }
        public DbSet<Persons> persons { get; set; }

        public override void Dispose()
        {
            Console.WriteLine("AppDbContext уничтожен: " + this.GetHashCode());
            base.Dispose();
        }
    }
}
