using System.Threading.Tasks;
using Models;

namespace Services.AdminRepositories
{
    public interface IAdminRepository
    {
        Task UpdateAppartments(Appartment model);
    }
}