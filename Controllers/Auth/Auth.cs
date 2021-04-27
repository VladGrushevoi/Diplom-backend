using Microsoft.AspNetCore.Mvc;
using Models;
using UseCase.Auth;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private AuthUseCase authUseCase;
        private IActionResult _output;

        public AuthController(AuthUseCase authUseCase)
        {
            this.authUseCase = authUseCase;
        }

        [HttpPost("registration")]
        public IActionResult AddNewUser([FromBody] User model)
        {
            _output = authUseCase.AddNewUser(model);
            if(_output == null)
            {
                return NotFound();
            }
            return Ok(_output);
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] User model)
        {
            _output = authUseCase.LoginUser(model);
            if(_output == null)
            {
                return NotFound();
            }

            return Ok(_output);
        }
    }
}