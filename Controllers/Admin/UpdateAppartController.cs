using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UseCase.Admin;

namespace Controllers.Admin
{
    [ApiController]
    [Route("api/admin")]
    public class UpdateAppartController : ControllerBase
    {
        public UpdateAppartUseCase updateAppartUseCase;
        private IActionResult _output = null;

        public UpdateAppartController(UpdateAppartUseCase updateAppartUseCase)
        {
            this.updateAppartUseCase = updateAppartUseCase;
        }

        [HttpGet("update-appartments")]
        public async Task<IActionResult> UpdateAppart()
        {
            _output = await updateAppartUseCase.UpdateAppartment(); 
            return _output;
        }
    }
}