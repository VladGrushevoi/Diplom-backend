using Microsoft.AspNetCore.Mvc;
using Models;
using Services.AdminRepositories;

namespace UseCase.Admin
{
    public class ImportantPlaceUseCase
    {
        private IAdminRepository adminRepository;

        public ImportantPlaceUseCase(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public IActionResult AddNewPlace(ImportantPlace model)
        {
            adminRepository.AddImportantPlace(model);
            return new JsonResult(new {Message = "Нове місце додано", Item = model});
        }

        public IActionResult DeletePlace(int id)
        {
            var place = adminRepository.DeletePlace(id).Result;
            return new JsonResult(new { Message = "Місце видалено", Item = place });
        }

        public IActionResult GetAllPlaces()
        {
            var places =  adminRepository.GetAllPlace().Result;
            return new JsonResult(new { places = places });
        }

        public IActionResult GetPlaceByDistrictName(string name)
        {
            var places = adminRepository.GetPlaceByDistrictName(name).Result;
            return new JsonResult(new { items = places });
        }

        public IActionResult GetOlaceById(int id)
        {
            var place = adminRepository.GetPlaceById(id).Result;
            return new JsonResult( new { place = place } );
        }
    }
}