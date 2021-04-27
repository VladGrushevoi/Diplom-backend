using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Services.DictionaryRepositories;

namespace UseCase
{
    public class InfoUseCase
    {
        private IDictionaryRepository dictionaryRepository;

        public InfoUseCase(IDictionaryRepository dictionaryRepository)
        {
            this.dictionaryRepository = dictionaryRepository;
        }

        public IActionResult GetDistrictsName()
        {
            return new JsonResult(new { Districts = dictionaryRepository.GetDistrics().Result });
        }

        public IActionResult GetTypePlaces()
        {
            return new JsonResult(new { Districts = dictionaryRepository.GetTypePlaces().Result });
        }

        public IActionResult SetDistrics()
        {
            dictionaryRepository.InitDistrics();
            return new JsonResult(new { Message = "Добавились райони" });
        }

        public IActionResult InitTypePlaces()
        {
            dictionaryRepository.InitTypePlaces();
            return new JsonResult(new { Message = "Добавились типи місць" });
        }
    }
}