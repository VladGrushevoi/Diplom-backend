using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using Services.AdminRepositories;
using Usecase.Admin.PredictorPrices;
using UseCase.Admin.PredictorPrices.Data;

namespace UseCase.Admin
{
    public class PredictPriceUseCase
    {
        private IAdminRepository adminRepository;
        private PredictorPrice predictor;

        public PredictPriceUseCase(IAdminRepository adminRepository, PredictorPrice predictor)
        {
            this.adminRepository = adminRepository;
            this.predictor = predictor;
        }

        public IActionResult GetPricePredict(ApartmentInput input)
        {
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.totalSquare.Value,
                RoomsCount = input.roomsCount.Value,
                Floor = input.floor.Value,
                DistrictValue = input.GetDistrictValueByName(input.districtName),
                Price = 0
            };
            float predictPrice = predictor.PredictPrice(apartmentSample);
            input.price = predictPrice;
            var apps = adminRepository.GetSimilarAppartments(apartmentSample).Result;
            //var similarList = FormSimilarAppsList(apps);
            return new JsonResult(new {Prediction = input, SimilarAppartments = apps});
        }

        private async Task<List<JObject>> FormSimilarAppsList(List<Appartment> portitableApps)
        {
            List<JObject> similarListApps = new List<JObject>();
            HttpClient _client = new HttpClient();
            foreach (var item in portitableApps)
            {
                string url = BuilderUrlByIdOrder(item.IdFromApi);
                var response = await _client.GetAsync(url);
                string data = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(data); 
                similarListApps.Add(json);   
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