using System;
using System.Collections.Generic;
using Models;
using Services.AdminRepositories;

namespace UseCase.Admin.PredictorPrices
{
    public class CustomPrediction
    {
        IAdminRepository adminRepository;
        List<Appartment> apps = new List<Appartment>();
        List<float> prices = new List<float>();
        float[] B = new float[5];
        List<float> errors = new List<float>();
        float error = 0.5f;
        float alpha = 0.01f;

        public CustomPrediction(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
            this.apps = adminRepository.GetAllApartment().Result;
            this.apps.ForEach(item => prices.Add(item.Price));
            Training();
        }

        private void Training()
        {
            for(int i = 0; i < apps.Count * 4; i++)
            {
                int idx = i % apps.Count;
                float p = -(B[0] +
                            B[1] * apps[idx].TotalSquare +
                            B[2] * apps[idx].RoomsCount +
                            B[3] * apps[idx].Floor +
                            B[4] * apps[idx].DistrictValue);
                float pred = Convert.ToSingle(1 / (1 + Math.Pow(2.71828f, p)));
                error = apps[idx].Price - pred;

                B[0] = B[0] - alpha * error * pred * (1 - pred) * 1.0f;
                B[1] = B[1] + alpha * error * pred * (1 - pred) * apps[idx].TotalSquare;
                B[2] = B[2] + alpha * error * pred * (1 - pred) * apps[idx].RoomsCount;
                B[3] = B[3] + alpha * error * pred * (1 - pred) * apps[idx].Floor;
                B[4] = B[4] + alpha * error * pred * (1 - pred) * apps[idx].DistrictValue;

                errors.Add(error);
            }
        }

        public void Test()
        {
            Appartment testApp = new Appartment(){
                TotalSquare = 20,
                RoomsCount = 1,
                Floor = 1,
                DistrictValue = 28
            };

            float pred = B[0] + B[1] * testApp.TotalSquare + 
                                B[2] * testApp.RoomsCount +
                                B[3] * testApp.Floor + 
                                B[4] * testApp.DistrictValue;

            System.Console.WriteLine("Predicted = " + pred);
        }
    }
}