using System.Threading.Tasks;
using Models;

namespace Services.AuthRepository
{
    public interface IAuthRepository
    {
        Task<User> AddNewUser(User model);
        User CheckUserOnLogin(User model);
    }
}