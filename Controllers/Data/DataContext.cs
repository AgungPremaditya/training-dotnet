using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Controllers.Entities;

namespace SuperHeroAPI.Controllers.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base (options) {}

        public DbSet<SuperHero> SuperHeroes { get; set; }
    }
}
