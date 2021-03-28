using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services.RieltorRepository
{
    public class RieltorRepository : BaseRepository, IRieltorRepository
    {
        public RieltorRepository(DbAppContext context) : base(context)
        {
        }

        public async Task<List<Appartment>> GetOrdersByLessPredictPrice(Appartment model)
        {
            return await context.apartment.Where(ap => (model.TotalSquare - 5 <= ap.TotalSquare && ap.TotalSquare <= model.TotalSquare + 5))
                                            .Where(ap => ap.DistrictValue == model.DistrictValue && ap.RoomsCount == model.RoomsCount)
                                            .Where(ap => ap.Price <= model.Price)
                                            .ToListAsync();
        }
    }
}