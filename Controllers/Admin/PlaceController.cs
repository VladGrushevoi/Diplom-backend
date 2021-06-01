using Microsoft.AspNetCore.Mvc;
using Models;
using UseCase.Admin;

namespace Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaceController : ControllerBase
    {
        private ImportantPlaceUseCase important;
        private IActionResult _output;

        public PlaceController(ImportantPlaceUseCase important)
        {
            this.important = important;
        }

        [HttpPost("add-place")]
        public IActionResult AddNewPlace([FromBody] ImportantPlace model)
        {
            _output = important.AddNewPlace(model);
            return Ok(_output);
        }

        [HttpDelete("{id}/delete-place")]
        public IActionResult DeletePlace([FromRoute] int id)
        {
            _output = important.DeletePlace(id);
            return Ok(_output);
        }

        [HttpGet("get-places")]
        public IActionResult GetAllPlaces()
        {
            _output = important.GetAllPlaces();
            return Ok(_output);
        }

        [HttpGet("{name}/places")]
        public IActionResult GetPlacesByNameDistrict([FromRoute] string name)
        {
            _output = important.GetPlaceByDistrictName(name);
            return Ok(_output);
        }

        [HttpGet("{id}")]
        public IActionResult GetPlaceById([FromRoute] int id)
        {
            _output = important.GetPlaceById(id);
            return Ok(_output);
        }
    }
}