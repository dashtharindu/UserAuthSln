namespace UserAuth.Models
{
    public class Photo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string photoUrl { get; set; }
        public string IsMain { get; set; } //todo - change this to boolean
    }
}