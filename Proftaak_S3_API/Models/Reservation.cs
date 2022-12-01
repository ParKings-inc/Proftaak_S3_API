using Proftaak_S3_API.Reservations;

namespace Proftaak_S3_API.Models {
    public class Reservation {
        public int Id { get; set; }
        public int? SpaceID { get; set; }
        public int CarID { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
