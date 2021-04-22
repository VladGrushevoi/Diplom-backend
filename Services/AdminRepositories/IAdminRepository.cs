using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.AdminRepositories
{
    public interface IAdminRepository
    {
        void UpdateAppartments(List<Appartment> models);
        Task<List<Appartment>> GetAllApartment();
        Task<List<Appartment>> GetSimilarAppartments(Appartment model);
        Task<int> DeleteAllAppartments();
        Task AddImportantPlace(ImportantPlace model);
        Task<ImportantPlace> DeletePlace(int id);
        Task<List<ImportantPlace>> GetPlaceByDistrictId(int id);
    }
}