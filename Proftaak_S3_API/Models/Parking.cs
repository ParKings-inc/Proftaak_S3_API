namespace Proftaak_S3_API.Models
{
    public class Parking
    {
        public int Id { get; set; }
        public Gerage Gerage { get; set; }
        public Auto Car { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public double? Price { get; set; }
    }
}
