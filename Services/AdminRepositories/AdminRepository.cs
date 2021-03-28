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

        public async Task<List<Appartment>> GetSimilarAppartments(Appartment model)
        {
            return await context.apartment.Where(ap => (model.TotalSquare - 5 <= ap.TotalSquare && ap.TotalSquare <= model.TotalSquare + 5)
                                                ).Where(ap => ap.DistrictValue == model.DistrictValue &&
                                                ap.RoomsCount == model.RoomsCount).ToListAsync();
        }

        public void UpdateAppartments(List<Appartment> models)
        {
            var uploader = new NpgsqlBulkUploader(context);
            uploader.Insert(models);
        }
    }
}