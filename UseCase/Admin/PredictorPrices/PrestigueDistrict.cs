using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Services.AdminRepositories;
using Services.DictionaryRepositories;

namespace UseCase.Admin.PredictorPrices
{
    public class PrestigueDistrict
    {
        private IAdminRepository adminRepository;
        private IDictionaryRepository dictionaryRepository;
        public Dictionary<string, double> DistrictToPrestigue;

        public PrestigueDistrict(IAdminRepository adminRepository, IDictionaryRepository dictionaryRepository)
        {
            this.adminRepository = adminRepository;
            this.dictionaryRepository = dictionaryRepository;
            DistrictToPrestigue = new Dictionary<string, double>();
            CalculatePrestigueDistrict();
        }

        public async Task<double> GetDistrictPrestigueValue(int idDistrict)
        {
            var nameDistrict = await adminRepository.GetDistrictById(idDistrict);
            return DistrictToPrestigue[nameDistrict.Name];
        }

        private void CalculatePrestigueDistrict()
        {
                var districts = dictionaryRepository.GetDistrics().Result;
                var importantPlace = adminRepository.GetAllPlace().Result;
                var typePlaces = dictionaryRepository.GetTypePlaces().Result;
                foreach (var district in districts)
                {
                    double prestigueRate = 1;
                    foreach (var typePlace in typePlaces)
                    {
                        prestigueRate += CalculateRate(district, typePlace, importantPlace) ? 0.005d : -0.005d;
                    }
                    DistrictToPrestigue.Add(district.Name, prestigueRate);
                    prestigueRate = 1;
                }
        }

        private bool CalculateRate(District distrcict, TypePlace typePlace, List<ImportantPlace> importantPlaces)
        {
            var placeCount = importantPlaces.Where(p => p.DistrictName == distrcict.Name && p.TypePlaceName == typePlace.Name);
            if(placeCount.Count() >= typePlace.Count){
                return true;
            }
            return false;
        }
    }
}