using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using UseCase.Search;

namespace Services.SearchRepository
{
    public interface ISearchRepository
    {
        Task<List<Appartment>> GetAppartmentByParameters(SearchInput _searchModel);
    }
}