using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using Services.RieltorRepository;
using Usecase.Admin.PredictorPrices;
using UseCase.Admin.PredictorPrices.Data;

namespace UseCase.Rieltor
{
    public class SearchPortitableAppsUseCase
    {
        private IRieltorRepository rieltorRepository;
        private PredictorPrice prediction;

        public SearchPortitableAppsUseCase(IRieltorRepository rieltorRepository, PredictorPrice prediction)
        {
            this.rieltorRepository = rieltorRepository;
            this.prediction = prediction;
        }

        public IActionResult GetPortitableOrders(ApartmentInput input)
        {
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.totalSquare.Value,
                RoomsCount = input.roomsCount.Value,
                Floor = input.floor.Value,
                DistrictValue = input.GetDistrictValueByName(input.districtName),
                Price = 0
            };
            float predictPrice = prediction.PredictPrice(apartmentSample);
            apartmentSample.Price = predictPrice;
            var portitableApparts = rieltorRepository.GetOrdersByLessPredictPrice(apartmentSample).Result;
            //var similarAppartments = FormSimilarAppsList(portitableApparts);
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