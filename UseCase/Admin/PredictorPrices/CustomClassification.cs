using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.AdminRepositories;
using UseCase.Admin.PredictorPrices.Data;

namespace UseCase.Admin.PredictorPrices
{
    public class CustomClassification
    {
        List<ClassificationModel> classificationModels = new List<ClassificationModel>();
        IAdminRepository adminRepository;
        PrestigueDistrict prestigue;

        

        public CustomClassification(IAdminRepository adminRepository, PrestigueDistrict prestigue)
        {
            this.classificationModels = new List<ClassificationModel>();
            this.adminRepository = adminRepository;
            this.prestigue = prestigue;
            var apps = adminRepository.GetAllApartment().Result;
            foreach (var item in apps)
            {
                this.classificationModels.Add(new ClassificationModel(item));
            }
        }

        public float PredictPrice(Appartment model)
        {
            //var result = new Tuple<float, Appartment>(0, null);
            var nearValue = new Tuple<float, Appartment>(float.MaxValue, null); 
            foreach (var item in classificationModels)
            {
                var currModels = item.GetDistanceToModel(model);
                // if (model.DistrictValue == currModels.Item2.DistrictValue)
                // {
                    if (Math.Abs(currModels.Item1) < Math.Abs(nearValue.Item1))
                    {
                        nearValue = currModels;
                    }
                // }
            }
            
            return nearValue.Item2.Price * (float)prestigue.GetDistrictPrestigueValue((int)model.DistrictValue).Result;
        }
    }
}