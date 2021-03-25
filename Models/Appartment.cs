using Microsoft.ML.Data;

namespace Models
{
    public class Appartment : BaseMosel
    {
        [ColumnName("TotalSquare")]
        public float TotalSquare { get; set; }
        [ColumnName("RoomsCount")]
        public float RoomsCount { get; set; }
        [ColumnName("DistrictValue")]
        public float DistrictValue { get; set; }
        [ColumnName("Floor")]
        public float Floor { get; set; }
        [ColumnName("Price")]
        public float Price { get; set; }

        public Appartment(){}
    }
}