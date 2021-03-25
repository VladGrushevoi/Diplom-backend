using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.AdminRepositories
{
    public interface IAdminRepository
    {
        void UpdateAppartments(List<Appartment> models);
        Task<List<Appartment>> GetAllApartment();
    }
}