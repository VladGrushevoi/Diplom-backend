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

        [HttpPost("get-portitable-orders/{methodType}")]
        public IActionResult GetPotritableOrders([FromBody] ApartmentInput input, [FromRoute] int methodType)
        {
            _output = searchPortitable.GetPortitableOrders(input, methodType);
            return Ok(_output);
        }
    }
}