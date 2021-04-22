using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
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
        private List<Appartment> aparts;
        private PredictionEngine<Appartment, ApartmentPrediction> predictionFunction;
        private RegressionMetrics metrics;

        public PredictorPrice(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
            this.mlContext = new MLContext(seed: 0);
            aparts = adminRepository.GetAllApartment().Result;
            if(aparts.Count != 0){
                this.model = Train(mlContext, aparts);
                this.predictionFunction = mlContext.Model.CreatePredictionEngine<Appartment, ApartmentPrediction>(model);
            }
        }

        public JsonResult TrainModel()
        {
            this.mlContext = new MLContext(seed: 0);
            aparts = adminRepository.GetAllApartment().Result;
            if(aparts.Count != 0){
                this.model = Train(mlContext, aparts);
                this.predictionFunction = mlContext.Model.CreatePredictionEngine<Appartment, ApartmentPrediction>(model);
                System.Console.WriteLine("Train model request");
            }
            return new JsonResult(new {
                Result = "Success"
                });
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

            IDataView dataView = mlContext.Data.LoadFromEnumerable<Appartment>(this.adminRepository.GetAllApartment().Result);

            var predictions = model.Transform(dataView);
            this.metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

            return model;
        }

        public Task<JsonResult> GetParametersModel()
        {
            return Task.Run(() => new JsonResult(new { 
                accurate = metrics.RSquared, 
                error = metrics.RootMeanSquaredError, 
                countApps = adminRepository.GetAllApartment().Result.Count 
                }));
        }

        private void Evaluate(MLContext mlContext, ITransformer model)
        {
            IDataView dataView = mlContext.Data.LoadFromEnumerable<Appartment>(this.adminRepository.GetAllApartment().Result);

            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError}");
            Console.WriteLine($"*       LossFunction:      {metrics.LossFunction}");
            Console.WriteLine($"*       MeanAbsoluteError:      {metrics.MeanAbsoluteError}");
            Console.WriteLine($"*       MeanSquaredError:      {metrics.MeanSquaredError}");
            Console.WriteLine($"*       MeanSquaredError:      {metrics}");
            Console.WriteLine($"*************************************************");
        }

        public float PredictPrice(Appartment model)
        {
            //Evaluate(this.mlContext, this.model);
            var prediction = predictionFunction.Predict(model);
            Console.WriteLine($"Predicted price: {prediction.Price}");
            //ToDo
            // зробить прорахунок відстані до ключовий місць
            //
            return prediction.Price;
        }
    }
}