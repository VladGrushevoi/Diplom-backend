using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Models;
using Services.AdminRepositories;
using UseCase.Admin.PredictorPrices.Data;

namespace UseCase.Admin.PredictorPrices
{
    public class ClasificationModel
    {
        private IAdminRepository adminRepository;
        private PrestigueDistrict prestigueDistrict;

        private MLContext _mlContext;
        private PredictionEngine<TrainModel, PredictClassModel> _predEngine;
        private ITransformer _trainedModel;
        private IDataView _trainingDataView;

        public ClasificationModel(IAdminRepository adminRepository, PrestigueDistrict prestigueDistrict)
        {
            this.prestigueDistrict = prestigueDistrict;
            this.adminRepository = adminRepository;
            this._mlContext = new MLContext(seed: 0);
            var apps = adminRepository.GetAllApartment().Result;
            this._trainingDataView = this._mlContext.Data.LoadFromEnumerable<TrainModel>(FormTrainListModels(apps));
            var pipeline = ProcessData();
            var trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);
            //Evaluate(_trainingDataView.Schema);
        }

        public IEstimator<ITransformer> ProcessData()
        {
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Price", outputColumnName: "Label")
                            .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "TotalSquare", outputColumnName: "TotalSquareFeaturized"))
                            .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "RoomsCount", outputColumnName: "RoomsCountFeaturized"))
                            .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Floor", outputColumnName: "FloorFeaturized"))
                            .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "DistrictValue", outputColumnName: "DistrictValueFeaturized"))
                            .Append(_mlContext.Transforms.Concatenate("Features", "TotalSquareFeaturized", "RoomsCountFeaturized", "FloorFeaturized","DistrictValueFeaturized"))
                            .AppendCacheCheckpoint(_mlContext);
            
            return pipeline;
        }

        public IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            var trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            _trainedModel = trainingPipeline.Fit(trainingDataView);
            _predEngine = _mlContext.Model.CreatePredictionEngine<TrainModel, PredictClassModel>(_trainedModel);
            return trainingPipeline;
        }

        public void Evaluate(DataViewSchema trainingDataViewSchema)
        {
            var testDataView = _mlContext.Data.LoadFromEnumerable<TrainModel>(FormTrainListModels(adminRepository.GetAllApartment().Result));
            var testMetrics = _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(testDataView));
            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model - Test Data     ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       MicroAccuracy:    {testMetrics.MicroAccuracy:0.###}");
            Console.WriteLine($"*       MacroAccuracy:    {testMetrics.MacroAccuracy:0.###}");
            Console.WriteLine($"*       LogLoss:          {testMetrics.LogLoss:#.###}");
            Console.WriteLine($"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}");
            Console.WriteLine($"*************************************************************************************************************");
        }

        public float PredictPrice(Appartment model)
        {
            var predictModel = new TrainModel{
                    TotalSquare = model.TotalSquare.ToString(),
                    RoomsCount = model.RoomsCount.ToString(),
                    DistrictValue = model.DistrictValue.ToString(),
                    Floor = model.Floor.ToString()
            };
            var prediction = _predEngine.Predict(predictModel);
            Console.WriteLine($"Predicted price: {prediction.Price}");
            var predictPrice = float.Parse(prediction.Price) * prestigueDistrict.GetDistrictPrestigueValue((int)model.DistrictValue).Result;
            
            return (float)predictPrice;
        }

        private List<TrainModel> FormTrainListModels(List<Appartment> apps)
        {
            List<TrainModel> newModels = new List<TrainModel>();
            for (int i = 0; i < apps.Count; i+=8)
            {
                newModels.Add(new TrainModel{
                    TotalSquare = apps[i].TotalSquare.ToString(),
                    RoomsCount = apps[i].RoomsCount.ToString(),
                    DistrictValue = apps[i].DistrictValue.ToString(),
                    Floor = apps[i].Floor.ToString(),
                    Price = apps[i].Price.ToString()
                });
            }
            return newModels;
        }
    }
}