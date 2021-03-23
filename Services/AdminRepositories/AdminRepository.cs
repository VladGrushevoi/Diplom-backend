using System.Threading.Tasks;
using Models;

namespace Services.AdminRepositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(DbAppContext context) : base(context)
        {
        }

        public async Task UpdateAppartments(Appartment model)
        {
            await context.apartment.AddAsync(model);
            await context.SaveChangesAsync();
        }
    }
}