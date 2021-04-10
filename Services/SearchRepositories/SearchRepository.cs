using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;
using UseCase.Search;

namespace Services.SearchRepository
{
    public class SearchRepository : BaseRepository, ISearchRepository
    {
        public SearchRepository(DbAppContext context) : base(context)
        {
        }

        public async Task<List<Appartment>> GetAppartmentByParameters(SearchInput _searchModel)
        {
            var appartments = await context.apartment.Where(ap => 
                                    _searchModel.TotalSquareFrom <= ap.TotalSquare && ap.TotalSquare <= _searchModel.TotalSquareTo
                                    && _searchModel.FloorFrom <= ap.Floor && ap.Floor <= _searchModel.FloorTo
                                    && _searchModel.PriceFrom <= ap.Price && ap.Price <= _searchModel.PriceTo
                                    ).ToListAsync();
            if(_searchModel.DistrictValue != 0)
            {
                appartments = appartments.Where(ap => ap.DistrictValue == _searchModel.DistrictValue).ToList();
            }
            if(_searchModel.RoomsCount.HasValue)
            {
                appartments = appartments.Where(ap => ap.RoomsCount == _searchModel.RoomsCount).ToList();
            }

           return appartments;
        }
    }
}