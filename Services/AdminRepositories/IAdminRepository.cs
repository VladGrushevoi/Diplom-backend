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
        Task<List<ImportantPlace>> GetPlaceByDistrictName(string nameDistrict);
        Task<List<ImportantPlace>> GetAllPlace();
        Task<ImportantPlace> GetPlaceById(int id);
        Task<District> GetDistrictById(int id);
        Task<District> GetDistrictByName(string name);
    }
}