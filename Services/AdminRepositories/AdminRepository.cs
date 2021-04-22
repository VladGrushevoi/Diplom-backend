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

        public async Task AddImportantPlace(ImportantPlace model)
        {
            await context.importantPlaces.AddAsync(model);
            context.SaveChanges();
        }

        public Task<int> DeleteAllAppartments()
        {
            return context.Database.ExecuteSqlRawAsync("Delete from apartment");
        }

        public async Task<ImportantPlace> DeletePlace(int id)
        {
             System.Console.WriteLine("delete id =" + id );
            ImportantPlace delPlace = context.importantPlaces.Where(item => item.Id == id).FirstOrDefault();
            context.importantPlaces.Remove(delPlace);
            await context.SaveChangesAsync();
            return delPlace;
        }

        

        public async Task<List<Appartment>> GetAllApartment()
        {
            return await context.apartment.ToListAsync();
        }

        public async Task<List<ImportantPlace>> GetPlaceByDistrictId(int id)
        {
            return await context.importantPlaces.Where(item => item.Id == id).ToListAsync();
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