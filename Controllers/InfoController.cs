using Microsoft.AspNetCore.Mvc;
using UseCase;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InfoController : ControllerBase
    {
        private IActionResult _output;
        private InfoUseCase infoUse;

        public InfoController(InfoUseCase infoUse)
        {
            this.infoUse = infoUse;
        }

        [HttpGet("districts")]
        public IActionResult GetDistricsName()
        {
            _output = infoUse.getDistrictsName();
            return _output;
        }
    }
}