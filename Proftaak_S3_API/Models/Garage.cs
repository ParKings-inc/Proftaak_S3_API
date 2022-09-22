namespace Proftaak_S3_API.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime? OpeningTime { get; set; }
        public DateTime? ClosingTime { get; set; }
        public int FreeSpace { get; set; }
        public int MaxSpace { get; set; }
    }
}
