using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using Services.AdminRepositories;
using Services.RieltorRepository;
using Usecase.Admin.PredictorPrices;
using UseCase.Admin.PredictorPrices;
using UseCase.Admin.PredictorPrices.Data;

namespace UseCase.Rieltor
{
    public class SearchPortitableAppsUseCase
    {
        private IRieltorRepository rieltorRepository;
        private IAdminRepository adminRepository;
        private PrestigueDistrict prestigue;

        public SearchPortitableAppsUseCase(IRieltorRepository rieltorRepository, IAdminRepository adminRepository, PrestigueDistrict prestigue)
        {
            this.rieltorRepository = rieltorRepository;
            this.adminRepository = adminRepository;
            this.prestigue = prestigue;
        }

        public IActionResult GetPortitableOrders(ApartmentInput input, int methodType)
        {
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.totalSquare.Value,
                RoomsCount = input.roomsCount.Value,
                Floor = input.floor.Value,
                DistrictValue = adminRepository.GetDistrictByName(input.districtName).Result.Id,
                Price = 0
            };
            float predictPrice = 0;
            switch(methodType){
                case 1:
                    predictPrice = new PredictorPrice(adminRepository, prestigue).PredictPrice(apartmentSample);
                    break;
                case 2:
                    double[,] data = new double[4, 1];
                    data[0, 0] = (double)input.totalSquare;
                    data[1, 0] = (double)input.roomsCount;
                    data[2, 0] = (double)adminRepository.GetDistrictByName(input.districtName).Result.Id;
                    data[3, 0] = (double)input.floor;
                    predictPrice = (float)new CustomPrediction(adminRepository, prestigue).Predict(data);
                    break;
                case 3:
                    predictPrice = new ClasificationModel(adminRepository, prestigue).PredictPrice(apartmentSample);
                    break;
                case 4:
                    predictPrice = new CustomClassification(adminRepository, prestigue).PredictPrice(apartmentSample);
                    break;
            }
            apartmentSample.Price = predictPrice;
            var portitableApparts = rieltorRepository.GetOrdersByLessPredictPrice(apartmentSample).Result;
            return new JsonResult(new { Prediction = apartmentSample, SimilarAppartments = portitableApparts });
        }

        private async Task<List<ActionResult>> FormSimilarAppsList(List<Appartment> portitableApps)
        {
            List<ActionResult> similarListApps = new List<ActionResult>();
            HttpClient _client = new HttpClient();
            foreach (var item in portitableApps)
            {
                string url = BuilderUrlByIdOrder(item.IdFromApi);
                var response = await _client.GetAsync(url);
                string data = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(data); 
                similarListApps.Add(json.ToObject<ActionResult>());   
            }
            return similarListApps;
        } 

        private string BuilderUrlByIdOrder(int id)
        {
            System.Console.WriteLine(id);
            return $"https://developers.ria.com/dom/info/{id}?api_key={Environment.GetEnvironmentVariable($"API_KEY_1")}";
        }
    }
}