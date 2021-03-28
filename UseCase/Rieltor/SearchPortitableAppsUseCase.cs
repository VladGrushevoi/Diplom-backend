using Microsoft.AspNetCore.Mvc;
using Models;
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
                TotalSquare = input.TotalSquare.Value,
                RoomsCount = input.RoomsCount.Value,
                Floor = input.Floor.Value,
                DistrictValue = input.GetDistrictValueByName(input.DistrictName),
                Price = 0
            };
            float predictPrice = prediction.PredictPrice(apartmentSample);
            apartmentSample.Price = predictPrice;
            var portitableApparts = rieltorRepository.GetOrdersByLessPredictPrice(apartmentSample).Result;
            return new JsonResult(new { SampleAppartment = apartmentSample, Orders = portitableApparts });
        }
    }
}