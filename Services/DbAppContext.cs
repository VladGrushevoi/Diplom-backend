using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class DbAppContext : DbContext
    {
        public DbSet<Appartment> apartment { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<District> districts { get; set; }
        public DbSet<TypePlace> typePlaces { get; set; }
        public DbSet<ImportantPlace> importantPlaces { get; set; }
        public DbAppContext(DbContextOptions opt) : base(opt)
        {

        }
    }
}