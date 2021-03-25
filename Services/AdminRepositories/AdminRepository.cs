using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql.Bulk;

namespace Services.AdminRepositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(DbAppContext context) : base(context)
        {
        }

        public async Task<List<Appartment>> GetAllApartment()
        {

            return await context.apartment.ToListAsync();
        }

        public void UpdateAppartments(List<Appartment> models)
        {
            var uploader = new NpgsqlBulkUploader(context);
            uploader.Insert(models);
        }
    }
}