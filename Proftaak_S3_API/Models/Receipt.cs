using Microsoft.EntityFrameworkCore;

namespace Proftaak_S3_API.Models
{
    public class Receipt
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        [Precision(9, 2)]
        public decimal Price { get; set; }
    }
}
