using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public class DbAppContext : DbContext
    {
        public DbSet<Appartment> apartment { get; set; }
        public DbAppContext(DbContextOptions opt) : base(opt)
        {

        }
    }
}