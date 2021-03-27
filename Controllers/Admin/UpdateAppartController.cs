using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Usecase.Admin.PredictorPrices;
using UseCase.Admin;
using UseCase.Admin.PredictorPrices;
using UseCase.Admin.PredictorPrices.Data;

namespace Controllers.Admin
{
    [ApiController]
    [Route("api/admin")]
    public class UpdateAppartController : ControllerBase
    {
        public UpdateAppartUseCase updateAppartUseCase;
        public PredictorPrice prediction;
        public CustomPrediction customPredictions;
        private IActionResult _output = null;

        public UpdateAppartController(UpdateAppartUseCase updateAppartUseCase, PredictorPrice predictor, CustomPrediction customPredictions)
        {
            this.updateAppartUseCase = updateAppartUseCase;
            this.prediction = predictor;
            this.customPredictions = customPredictions;
        }

        [HttpGet("update-appartments")]
        public async Task<IActionResult> UpdateAppart()
        {
            _output = await updateAppartUseCase.UpdateAppartment(); 
            return _output;
        }

        [HttpGet("predict")]
        public IActionResult CalculatePrice([FromBody] ApartmentInput input)
        {
            input.Price = prediction.TestSinglePrediction(input);
            return Ok(input);
        }
    }
}