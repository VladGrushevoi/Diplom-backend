using System.Collections.Generic;
using Microsoft.ML.Data;

namespace UseCase.Admin.PredictorPrices.Data
{
    public class ApartmentInput
    {
        public float? totalSquare { get; set; }
        public int? roomsCount { get; set; }
        public float? price { get; set; }
        public int? floor { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string districtName { get; set; }

        public float GetDistrictValueByName(string v)
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

    public class ApartmentPrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}