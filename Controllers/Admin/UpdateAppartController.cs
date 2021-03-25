using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Usecase.Admin.PredictorPrices;
using UseCase.Admin;

namespace Controllers.Admin
{
    [ApiController]
    [Route("api/admin")]
    public class UpdateAppartController : ControllerBase
    {
        public UpdateAppartUseCase updateAppartUseCase;
        public PredictorPrice prediction;
        private IActionResult _output = null;

        public UpdateAppartController(UpdateAppartUseCase updateAppartUseCase, PredictorPrice predictor)
        {
            this.updateAppartUseCase = updateAppartUseCase;
            this.prediction = predictor;
        }

        [HttpGet("update-appartments")]
        public async Task<IActionResult> UpdateAppart()
        {
            _output = await updateAppartUseCase.UpdateAppartment(); 
            return _output;
        }

        [HttpGet("predict")]
        public IActionResult CalculatePrice()
        {
            prediction.CalculateModel();
            return Ok("Заїбісь чекай консоль");
        }
    }
}