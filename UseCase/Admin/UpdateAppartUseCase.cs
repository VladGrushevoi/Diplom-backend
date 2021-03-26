using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.AdminRepositories;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using Models;
using System;

namespace UseCase.Admin
{
    public class UpdateAppartUseCase
    {
        public IAdminRepository adminRepo;
        private HttpClient _client;
        private string[] OrdersUrlApi = new string[]{
            @"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=1000&characteristic[234][to]=26000&API_KEY=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH",
            @"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=26001&characteristic[234][to]=32999&API_KEY=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH",
            @"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=33000&characteristic[234][to]=38500&API_KEY=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH",
            @"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=38501&characteristic[234][to]=50500&API_KEY=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH",
            @"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=50501&characteristic[234][to]=75000&API_KEY=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH",
            @"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=75001&characteristic[234][to]=9999999&API_KEY=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH"
        };

        public UpdateAppartUseCase(IAdminRepository adminRepo)
        {
            this.adminRepo = adminRepo;
            this._client = new HttpClient();
        }

        public async Task<IActionResult> UpdateAppartment()
        {
            foreach (var item in this.OrdersUrlApi)
            {
                var idOrders = await GetOrdersId(item);
                List<Appartment> apparts = new List<Appartment>();
                foreach (var id in idOrders)
                {
                    try
                    {
                        Appartment appart = await CreateAppart(id);
                        apparts.Add(appart);
                    }
                    catch (System.Exception)
                    {
                        continue;
                    }
                }
                adminRepo.UpdateAppartments(apparts);
                apparts.Clear();
            }
            return new JsonResult("НУ тіпа");
        }

        private async Task<Appartment> CreateAppart(JToken jToken)
        {
            string url = BuilderUrlByIdOrder(jToken.ToString());
            var response = await _client.GetAsync(url);
            string data = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(data);
            Appartment appartTemp = new Appartment()
            {
                TotalSquare = float.Parse(json["total_square_meters"].ToString()),
                RoomsCount = float.Parse(json["rooms_count"].ToString()),
                DistrictValue = GetDistrictValueByName(json["district_name"].ToString()),
                Price = float.Parse(json["priceArr"]["1"].ToString()),
                Floor = float.Parse(json["floor"].ToString())
            };

            return appartTemp;
        }

        private float GetDistrictValueByName(string v)
        {
            List<string> districts = new List<string>(){
                "700-летия","Благовесный","Богдановский","Водоконал-Невского","Грузовой порт",
                "Дахновка","Днепровский","Железнодорожний вокзал","Зеленый","к-т Мир","Казбет",
                "Калиновский","Крываловский","Луначарский","Молокозавод","Мытница","Мытница-речпорт",
                "Мытница-центр","Пацаева","Победа","Приднепровский","Припортовый","Пятихатки","Район Д",
                "Самолет","Седова","Соборный","Сосновка","Сосновский","Стадион","Химпоселок","Центр",
                "Черкасский","Школьная","ЮЗР","Яблочный","Пригород","Белозерье","Геронимовка","Оршанец",
                "Русская Поляна","Червоная Слобода","Село","Байбузы","Березняки","Крещатик","Леськи","Лозовок",
                "Мошногорье","Мошны","Нечаевка","Новосёловка","Первомайское","Сагуновка","Светанок","Свидивок",
                "Сокирно","Софиевка","Степанки","Тубольцы","Хацьки","Худяки","Хутора","Чернявка","Шелепухи","Яснозорье"
            };
            
            if(districts.Contains(v))
            {
                return districts.IndexOf(v) + 1;
            }
            return 0;
        }

        private async Task<List<JToken>> GetOrdersId(string url)
        {
            var response = await _client.GetAsync(url);
            System.Console.WriteLine(response.StatusCode);
            string data = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(data);
            var items = json["items"].ToList();
            return items;
        }
        private string BuilderUrlByIdOrder(string id)
        {
            System.Console.WriteLine(id);
            return $"https://developers.ria.com/dom/info/{id}?api_key=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH";
        }
    }
}