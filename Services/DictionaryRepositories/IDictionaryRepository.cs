using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services.DictionaryRepositories
{
    public interface IDictionaryRepository
    {
        Task<List<District>> GetDistrics();
        void InitDistrics();
        Task<List<TypePlace>> GetTypePlaces();
        void InitTypePlaces();

    }
}