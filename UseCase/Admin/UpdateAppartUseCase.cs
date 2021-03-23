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
                foreach (var id in idOrders)
                {
                    Appartment appart = await CreateAppart(id);
                    await adminRepo.UpdateAppartments(appart);
                }
            }
            return new JsonResult("НУ тіпа");
        }

        private async Task<Appartment> CreateAppart(JToken jToken)
        {
            string url = BuilderUrlByIdOrder(jToken.ToString());
            var response = await _client.GetAsync(url);
            string data = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(data);
            System.Console.WriteLine((double)json["total_square_meters"]);
            Appartment appartTemp = new Appartment()
            {
                TotalSquare = Double.Parse(json["total_square_meters"].ToString()),
                RoomsCount = Int32.Parse(json["rooms_count"].ToString()),
                StreetName = json["street_name"].ToString(),
                Price = Double.Parse(json["priceArr"]["1"].ToString()),
                Floor = Int32.Parse(json["floor"].ToString())
            };

            return appartTemp;
        }

        private async Task<List<JToken>> GetOrdersId(string url)
        {
            var response = await _client.GetAsync(url);
            string data = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(data);
            var items = json["items"].ToList();
            return items;
        }
        private string BuilderUrlByIdOrder(string id)
        {
            return $"https://developers.ria.com/dom/info/{id}?api_key=FJJUHJypHZaO9VupoliHJtalBWEtL1UQ4NEjnDFH";
        }
    }
}