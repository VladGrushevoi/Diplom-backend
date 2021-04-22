namespace Models
{
    public class User : BaseMosel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }
}