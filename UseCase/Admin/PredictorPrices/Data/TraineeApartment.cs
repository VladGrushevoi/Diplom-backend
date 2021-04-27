using System.Collections.Generic;
using Microsoft.ML.Data;

namespace UseCase.Admin.PredictorPrices.Data
{
    public class ApartmentInput
    {
        public float? totalSquare { get; set; }
        public int? roomsCount { get; set; }
        public float? price { get; set; }
        public int? floor { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string districtName { get; set; }
    }

    public class ApartmentPrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}