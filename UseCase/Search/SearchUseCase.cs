using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.SearchRepository;

namespace UseCase.Search
{
    public class SearchUseCase
    {
        private ISearchRepository _searchRepository;

        public SearchUseCase(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public IActionResult SearchExecute(SearchInput _searchModel)
        {
            if(_searchModel == null){
                return new JsonResult(new {Message="Error"});
            }
            _searchModel.DistrictValue = GetDistrictValueByName(_searchModel.DistrictName);
            var appartments = _searchRepository.GetAppartmentByParameters(_searchModel);

            return new JsonResult(new {Appartments = appartments.Result});
        }

        private int GetDistrictValueByName(string v)
        {
            List<string> districts = new List<string>(){
                "700-летия","Благовесный","Богдановский","Водоконал-Невского","Грузовой порт",
                "Дахновка","Днепровский","Железнодорожний вокзал","Зеленый","к-т Мир","Казбет",
                "Калиновский","Крываловский","Луначарский","Молокозавод","Мытница","Мытница-речпорт",
                "Мытница-центр","Пацаева","Победа","Приднепровский","Припортовый","Пятихатки","Район Д",
                "Самолет","Седова","Соборный","Сосновка","Сосновский","Стадион","Химпоселок","Центр",
                "Черкасский","Школьная","ЮЗР",
                // "Яблочный","Пригород","Белозерье","Геронимовка","Оршанец",
                // "Русская Поляна","Червоная Слобода","Село","Байбузы","Березняки","Крещатик","Леськи","Лозовок",
                // "Мошногорье","Мошны","Нечаевка","Новосёловка","Первомайское","Сагуновка","Светанок","Свидивок",
                // "Сокирно","Софиевка","Степанки","Тубольцы","Хацьки","Худяки","Хутора","Чернявка","Шелепухи","Яснозорье",
                // "Будище", "Ирдынь"
            };

            if(districts.Contains(v))
            {
                return districts.IndexOf(v) + 1;
            }
            return 0;
        }
    }
}