namespace Models
{
    public class Appartment : BaseMosel
    {
        public double TotalSquare { get; set; }
        public int RoomsCount { get; set; }
        public string StreetName { get; set; }
        public int Floor { get; set; }
        public double Price { get; set; }

        public Appartment(){}
    }
}