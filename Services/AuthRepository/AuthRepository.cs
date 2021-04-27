using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Services.AuthRepository
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(DbAppContext context) : base(context)
        {
        }

        public async Task<User> AddNewUser(User model)
        {
            var users = context.user.Where(u => u.Email == model.Email &&
                                            u.Status == model.Status);
            if (users.Count() != 0)
            {
                return null;
            }else{
                users = context.user.Where(u => u.Email == model.Email &&
                                            u.Password == model.Password);
            }
            if(users.Count() != 0)
            {
                return null;
            }
            var result = context.user.Add(model);
            var user = result.Entity;
            await context.SaveChangesAsync();
            return user;
        }
        public User CheckUserOnLogin(User model)
        {
            var user = context.user.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            System.Console.WriteLine("Login "+ model.Email + " "+ model.Password);
            if(user is null)
            {
                return null;
            }
            return user;
        }
    }
}