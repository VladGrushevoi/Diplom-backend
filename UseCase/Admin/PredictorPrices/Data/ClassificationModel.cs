using System;
using Models;

namespace UseCase.Admin.PredictorPrices.Data
{
    public class ClassificationModel
    {
        private Appartment app;

        public ClassificationModel(Appartment app)
        {
            this.app = app;
        }

        public Tuple<float, Appartment> GetDistanceToModel(Appartment model)
        {
            var diff = Math.Pow((model.TotalSquare - app.TotalSquare), 2) + Math.Pow((model.RoomsCount - app.RoomsCount), 2) +
                            Math.Pow((model.Floor - app.Floor), 2) + Math.Pow((model.DistrictValue - app.DistrictValue), 2);
            System.Console.WriteLine(diff);
            var distance = Math.Sqrt(diff);
            var result = new Tuple<float, Appartment>((float)distance, app);
            return result;
        }

        // public Tuple<bool, Appartment> CheckApps(Appartment model)
        // {
        //     if(app.TotalSquare - 5 <= model.TotalSquare && app.TotalSquare <= model.TotalSquare + 5 )
        //     {
        //         if(app.RoomsCount == model.RoomsCount)
        //         {
        //             if(app.Floor - 3 <= model.Floor && model.Floor <= app.Floor + 3)
        //             {
        //                 if(model.DistrictValue == app.DistrictValue)
        //                 {
        //                     return new Tuple<bool, Appartment>(false, app);
        //                 }
        //             }else
        //             {
        //                 return new Tuple<bool, Appartment>(false, null);
        //             }
        //         }else
        //         {
        //             return new Tuple<bool, Appartment>(false, null);
        //         }
        //     }
            
        //     return new Tuple<bool, Appartment>(false, null);
        // }
    }
}