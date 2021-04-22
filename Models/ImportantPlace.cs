namespace Models
{
    public class ImportantPlace : BaseMosel
    {
        public int DistrictId { get; set; }
        public int TypePlaceId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        
    }
}