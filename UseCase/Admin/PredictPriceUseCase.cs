using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using Services.AdminRepositories;
using Usecase.Admin.PredictorPrices;
using UseCase.Admin.PredictorPrices;
using UseCase.Admin.PredictorPrices.Data;

namespace UseCase.Admin
{
    public class PredictPriceUseCase
    {
        private IAdminRepository adminRepository;
        private PrestigueDistrict prestigue;

        public PredictPriceUseCase(IAdminRepository adminRepository, PrestigueDistrict prestigue)
        {
            this.adminRepository = adminRepository;
            this.prestigue = prestigue;
        }

        public IActionResult GetPricePredict(ApartmentInput input)
        {
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.totalSquare.Value,
                RoomsCount = input.roomsCount.Value,
                Floor = input.floor.Value,
                DistrictValue = adminRepository.GetDistrictByName(input.districtName).Result.Id,
                Price = 0
            };
            float predictPrice = new PredictorPrice(adminRepository, prestigue).PredictPrice(apartmentSample);
            input.price = predictPrice;
            var apps = adminRepository.GetSimilarAppartments(apartmentSample).Result;
            //var similarList = FormSimilarAppsList(apps);
            return new JsonResult(new {Prediction = input, SimilarAppartments = apps});
        }

        public IActionResult ClassificationPredict(ApartmentInput input)
        {
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.totalSquare.Value,
                RoomsCount = input.roomsCount.Value,
                Floor = input.floor.Value,
                DistrictValue = adminRepository.GetDistrictByName(input.districtName).Result.Id,
                Price = 0
            };
             float predictPrice = new ClasificationModel(adminRepository, prestigue).PredictPrice(apartmentSample);
            input.price = predictPrice;
            var apps = adminRepository.GetSimilarAppartments(apartmentSample).Result;
            return new JsonResult(new {Prediction = input, SimilarAppartments = apps});
        }

        public IActionResult CustomClassificationPredict(ApartmentInput input)
        {
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.totalSquare.Value,
                RoomsCount = input.roomsCount.Value,
                Floor = input.floor.Value,
                DistrictValue = adminRepository.GetDistrictByName(input.districtName).Result.Id,
                Price = 0
            };
             float predictPrice = new CustomClassification(adminRepository, prestigue).PredictPrice(apartmentSample);
            input.price = predictPrice;
            var apps = adminRepository.GetSimilarAppartments(apartmentSample).Result;
            return new JsonResult(new {Prediction = input, SimilarAppartments = apps});
        }

        public IActionResult TestCustomPrediction(ApartmentInput input)
        {
            double[,] data = new double[4,1];
            data[0,0] = (double)input.totalSquare;
            data[1, 0] = (double)input.roomsCount;
            data[2, 0] = (double)adminRepository.GetDistrictByName(input.districtName).Result.Id;
            data[3, 0] = (double)input.floor;
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.totalSquare.Value,
                RoomsCount = input.roomsCount.Value,
                Floor = input.floor.Value,
                DistrictValue = adminRepository.GetDistrictByName(input.districtName).Result.Id,
                Price = 0
            };
            double predictPrice = new CustomPrediction(adminRepository, prestigue).Predict(data);
            input.price = (float)predictPrice;
            var apps = adminRepository.GetSimilarAppartments(apartmentSample).Result;
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