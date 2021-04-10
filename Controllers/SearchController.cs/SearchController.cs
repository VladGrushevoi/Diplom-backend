using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UseCase.Search;

namespace Controllers.Search
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private SearchUseCase _searchUseCase;
        private IActionResult _output;

        public SearchController(SearchUseCase searchUseCase)
        {
            _searchUseCase = searchUseCase;
        }

        [HttpPost("apartments")]
        public IActionResult SearchAppartments([FromBody] SearchInput _searchModel)
        {   
            _output = _searchUseCase.SearchExecute(_searchModel);
            return _output;
        }
    }
}