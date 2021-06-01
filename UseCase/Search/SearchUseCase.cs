using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.AdminRepositories;
using Services.SearchRepository;

namespace UseCase.Search
{
    public class SearchUseCase
    {
        private ISearchRepository _searchRepository;
        private IAdminRepository _adminRepository;

        public SearchUseCase(ISearchRepository searchRepository, IAdminRepository adminRepository)
        {
            this._searchRepository = searchRepository;
            this._adminRepository = adminRepository;
        }

        public IActionResult SearchExecute(SearchInput _searchModel)
        {
            if(_searchModel == null){
                return new JsonResult(new {Message="Error"});
            }
            if(_searchModel.DistrictName != "")
            {
                System.Console.WriteLine(_searchModel.DistrictName+" hwbedjwebdw");
                _searchModel.DistrictValue = _adminRepository.GetDistrictByName(_searchModel.DistrictName).Result.Id;
            }
            var appartments = _searchRepository.GetAppartmentByParameters(_searchModel);

            return new JsonResult(new {Appartments = appartments.Result});
        }
    }
}