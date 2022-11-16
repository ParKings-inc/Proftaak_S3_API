namespace Proftaak_S3_API.Models
{
    public class Reservations
    {
        public int Id { get; set; }
        public int? SpaceID { get; set; }
        public int CarID { get; set; }
        public string Status { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
