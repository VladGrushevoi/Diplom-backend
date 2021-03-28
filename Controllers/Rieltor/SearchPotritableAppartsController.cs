using Microsoft.AspNetCore.Mvc;
using UseCase.Admin.PredictorPrices.Data;
using UseCase.Rieltor;

namespace Controllers.Rieltor
{
    [ApiController]
    [Route("api/rieltor")]
    public class SerchPortitableAppsController : ControllerBase
    {
        private SearchPortitableAppsUseCase searchPortitable;
        private IActionResult _output;

        public SerchPortitableAppsController(SearchPortitableAppsUseCase searchPortitable)
        {
            this.searchPortitable = searchPortitable;
        }

        [HttpGet("get-portitable-orders")]
        public IActionResult GetPotritableOrders([FromBody] ApartmentInput input)
        {
            _output = searchPortitable.GetPortitableOrders(input);
            return Ok(_output);
        }
    }
}