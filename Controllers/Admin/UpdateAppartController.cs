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
        public PredictPriceUseCase predictionUseCase;
        private IActionResult _output = null;

        public UpdateAppartController(UpdateAppartUseCase updateAppartUseCase, PredictPriceUseCase predictor)
        {
            this.updateAppartUseCase = updateAppartUseCase;
            this.predictionUseCase = predictor;
        }

        [HttpGet("update-appartments")]
        public async Task<IActionResult> UpdateAppart()
        {
            _output = await updateAppartUseCase.UpdateAppartment(); 
            return _output;
        }

        [HttpPost("predict")]
        public IActionResult CalculatePrice([FromBody] ApartmentInput input)
        {
            _output = predictionUseCase.GetPricePredict(input);
            return Ok(_output);
        }

        [HttpDelete("delete-apartments")]
        public IActionResult DeleteApartments()
        {
            _output = updateAppartUseCase.DeleteApartmentsUseCase().Result;
            return _output;
        }

        [HttpPost("train-model")]
        public IActionResult TrainModel()
        {
            _output = updateAppartUseCase.TrainModelUseCase();
            return _output;
        }

        [HttpGet("model-parameters")]
        public IActionResult GetParametersFromModel()
        {
            _output = updateAppartUseCase.GetParametersModel().Result;
            return _output;
        }
    }
}