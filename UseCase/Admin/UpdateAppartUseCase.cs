using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.AdminRepositories;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using Models;
using System;
using Usecase.Admin.PredictorPrices;

namespace UseCase.Admin
{
    public class UpdateAppartUseCase
    {
        public IAdminRepository adminRepo;
        private HttpClient _client;
        private PredictorPrice prediction;
        private string[] OrdersUrlApi = new string[]{
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=1000&characteristic[234][to]=25000&API_KEY={Environment.GetEnvironmentVariable("API_KEY_1")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=25001&characteristic[234][to]=30000&API_KEY={Environment.GetEnvironmentVariable("API_KEY_2")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=30001&characteristic[234][to]=32000&API_KEY={Environment.GetEnvironmentVariable("API_KEY_3")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=32001&characteristic[234][to]=34500&API_KEY={Environment.GetEnvironmentVariable("API_KEY_4")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=34501&characteristic[234][to]=40000&API_KEY={Environment.GetEnvironmentVariable("API_KEY_5")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=40001&characteristic[234][to]=45000&API_KEY={Environment.GetEnvironmentVariable("API_KEY_6")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=45001&characteristic[234][to]=50300&API_KEY={Environment.GetEnvironmentVariable("API_KEY_5")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=50301&characteristic[234][to]=73000&API_KEY={Environment.GetEnvironmentVariable("API_KEY_6")}",
            $"https://developers.ria.com/dom/search?category=1&realty_type=2&operation_type=1&state_id=24&city_id=24&characteristic[234][from]=73001&characteristic[234][to]=9000000000000&API_KEY={Environment.GetEnvironmentVariable("API_KEY_1")}"
        };

        internal IActionResult TrainModelUseCase()
        {
            return prediction.TrainModel();
        }

        public UpdateAppartUseCase(IAdminRepository adminRepo, PredictorPrice predictor)
        {
            this.adminRepo = adminRepo;
            this.prediction = predictor;
            this._client = new HttpClient();
        }

        public async Task<IActionResult> UpdateAppartment()
        {
            int amountApps = 0;
            int idApi = 1;
            List<Appartment> apparts = new List<Appartment>();
            foreach (var item in this.OrdersUrlApi)
            {
                var idOrders = await GetOrdersId(item);
                System.Console.WriteLine("API KEY #"+idApi);
                foreach (var id in idOrders)
                {
                    if(idApi >= 7){
                        idApi = 1;
                    }
                    try
                    {
                        Appartment appart = await CreateAppart(id, idApi);
                        apparts.Add(appart);
                    }
                    catch (System.Exception)
                    {
                        continue;
                    }
                }
                idApi++;
            }
            idApi = 1;
            amountApps = apparts.Count;
            adminRepo.UpdateAppartments(apparts);
            apparts.Clear();
            return new JsonResult(new {Amount = amountApps});
        }

        public async Task<IActionResult> DeleteApartmentsUseCase()
        {
            int countsDeleted = await adminRepo.DeleteAllAppartments();
            return new JsonResult(new { countDeleted = countsDeleted });
        }

        public async Task<IActionResult> GetParametersModel()
        {
            return await prediction.GetParametersModel();
        }

        private async Task<Appartment> CreateAppart(JToken jToken, int idApi)
        {
            string url = BuilderUrlByIdOrder(jToken.ToString(), idApi);
            var response = await _client.GetAsync(url);
            string data = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(data);
            Appartment appartTemp = new Appartment()
            {
                IdFromApi = Int32.Parse(json["realty_id"].ToString()),
                TotalSquare = float.Parse(json["total_square_meters"].ToString()),
                RoomsCount = float.Parse(json["rooms_count"].ToString()),
                DistrictValue = adminRepo.GetDistrictByName(json["district_name"].ToString()).Result.Id,
                Price = float.Parse(json["priceArr"]["1"].ToString()),
                Floor = float.Parse(json["floor"].ToString())
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
        private string BuilderUrlByIdOrder(string id, int idApi)
        {
            System.Console.WriteLine(id);
            return $"https://developers.ria.com/dom/info/{id}?api_key={Environment.GetEnvironmentVariable($"API_KEY_{idApi}")}";
        }
    }
}