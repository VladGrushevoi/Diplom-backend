using System.Collections.Generic;
using Models;
using Npgsql.Bulk;

namespace Services.AdminRepositories
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(DbAppContext context) : base(context)
        {
        }

        public void UpdateAppartments(List<Appartment> models)
        {
            var uploader = new NpgsqlBulkUploader(context);
            uploader.Insert(models);
        }
    }
}