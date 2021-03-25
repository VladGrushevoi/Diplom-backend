using Microsoft.ML.Data;

namespace UseCase.Admin.PredictorPrices.Data
{
    public class TraineeApartment
    {
        public float TotalSquare { get; set; }
        public int RoomsCount { get; set; }
        public float Price { get; set; }
        public int Floor { get; set; }
    }

    public class ApartmentPrediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }
}