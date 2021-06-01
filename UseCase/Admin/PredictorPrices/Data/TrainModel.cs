using Microsoft.ML.Data;

namespace UseCase.Admin.PredictorPrices.Data
{
    public class TrainModel
    {
        [ColumnName("Floor")]
        public string Floor { get; set; }
        [ColumnName("TotalSquare")]
        public string TotalSquare { get; set; }
        [ColumnName("RoomsCount")]
        public string RoomsCount { get; set; }
        [ColumnName("DistrictValue")]
        public string DistrictValue { get; set; }
        [ColumnName("Price")]
        public string Price { get; set; }
    }

    public class PredictClassModel
    {
        [ColumnName("PredictedLabel")]
        public string Price;
    }
}