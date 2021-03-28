using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Models;
using Services.AdminRepositories;
using UseCase.Admin.PredictorPrices.Data;

namespace Usecase.Admin.PredictorPrices
{
    public class PredictorPrice
    {
        private IAdminRepository adminRepository;
        private MLContext mlContext;
        private ITransformer model;
        private PredictionEngine<Appartment, ApartmentPrediction> predictionFunction;

        public PredictorPrice(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
            this.mlContext = new MLContext(seed: 0);
            var aparts = adminRepository.GetAllApartment().Result;
            this.model = Train(mlContext, aparts);
            this.predictionFunction = mlContext.Model.CreatePredictionEngine<Appartment, ApartmentPrediction>(model);
        }

        private ITransformer Train(MLContext mlContext, List<Appartment> aparts)
        {
            IDataView data = mlContext.Data.LoadFromEnumerable<Appartment>(aparts);
            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "Price")
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "TotalSquareEncoded", inputColumnName: "TotalSquare"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RoomsCountEncoded", inputColumnName: "RoomsCount"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "FloorEncoded", inputColumnName: "Floor"))
                    .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "DistrictValueEncoded", inputColumnName: "DistrictValue"))
                    .Append(mlContext.Transforms.Concatenate("Features", "TotalSquareEncoded", "RoomsCountEncoded", "FloorEncoded", "DistrictValueEncoded"))
                    .Append(mlContext.Regression.Trainers.FastTree());

            var model = pipeline.Fit(data);
            return model;
        }

        // private void Evaluate(MLContext mlContext, ITransformer model)
        // {
        //     IDataView dataView = mlContext.Data.LoadFromEnumerable<Appartment>(this.adminRepository.GetAllApartment().Result);

        //     var predictions = model.Transform(dataView);
        //     var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

        //     Console.WriteLine();
        //     Console.WriteLine($"*************************************************");
        //     Console.WriteLine($"*       Model quality metrics evaluation         ");
        //     Console.WriteLine($"*------------------------------------------------");
        //     Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
        //     Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError}");
        //     Console.WriteLine($"*************************************************");
        // }

        public IActionResult PredictPrice(ApartmentInput input)
        {
            var apartmentSample = new Appartment()
            {
                TotalSquare = input.TotalSquare.Value,
                RoomsCount = input.RoomsCount.Value,
                Floor = input.Floor.Value,
                DistrictValue = input.GetDistrictValueByName(input.DistrictName),
                Price = 0
            };
            var prediction = predictionFunction.Predict(apartmentSample);
            Console.WriteLine($"Predicted price: {prediction.Price}");
            input.Price = prediction.Price;
            return new JsonResult(new {Prediction = input, SimilarAppartments = adminRepository.GetSimilarAppartments(apartmentSample).Result});
        }
    }
}