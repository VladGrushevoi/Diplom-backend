using Microsoft.AspNetCore.Mvc;
using Models;
using Services.AuthRepository;

namespace UseCase.Auth
{
    public class AuthUseCase
    {
        private IAuthRepository authRepository;

        public AuthUseCase(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        public IActionResult AddNewUser(User model)
        {
            var result = authRepository.AddNewUser(model).Result;
            if(result == null)
            {   
                return null;
            }
            return new JsonResult(new {result});
        }

        public IActionResult LoginUser(User model)
        {
            var result = authRepository.CheckUserOnLogin(model);
            if(result == null){
                return null;
            }
            return new JsonResult(result);
        }
    }
}