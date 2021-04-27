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
            _output = infoUse.GetDistrictsName();
            return _output;
        }

        [HttpGet("init-districts")]
        public IActionResult InitDistricts()
        {
            _output = infoUse.SetDistrics();
            return _output;
        }

        [HttpGet("type-places")]
        public IActionResult GetTypePlaces()
        {
            _output = infoUse.GetTypePlaces();
            return _output;
        }

        [HttpGet("init-type-places")]
        public IActionResult InitTypePlaces()
        {
            _output = infoUse.InitTypePlaces();
            return _output;
        }
    }
}