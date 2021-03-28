using Microsoft.AspNetCore.Mvc;
using Models;
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
                TotalSquare = input.TotalSquare.Value,
                RoomsCount = input.RoomsCount.Value,
                Floor = input.Floor.Value,
                DistrictValue = input.GetDistrictValueByName(input.DistrictName),
                Price = 0
            };
            float predictPrice = predictor.PredictPrice(apartmentSample);
            input.Price = predictPrice;
            return new JsonResult(new {Prediction = input, SimilarAppartments = adminRepository.GetSimilarAppartments(apartmentSample).Result});
        }
    }
}