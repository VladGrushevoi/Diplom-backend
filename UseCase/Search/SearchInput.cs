namespace UseCase.Search
{
    public class SearchInput
    {
        public float TotalSquareFrom { get; set; }
        public float TotalSquareTo { get; set; }
        public float FloorFrom { get; set; }
        public float FloorTo { get; set; }
        public int? RoomsCount { get; set; }
        public float PriceFrom { get; set; }
        public int PriceTo { get; set; }
        public string DistrictName { get; set; }
        public int DistrictValue { get; set; }
    }
}