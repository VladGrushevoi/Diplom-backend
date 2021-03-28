using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.RieltorRepository
{
    public interface IRieltorRepository
    {
        Task<List<Appartment>> GetOrdersByLessPredictPrice(Appartment model);
    }
}