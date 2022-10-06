using Microsoft.EntityFrameworkCore;

namespace Proftaak_S3_API.Models
{
    public class Pricing
    {
        public int ID { get; set; }
        public int GarageID { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        [Precision(9, 2)]
        public decimal Price { get; set; }
    }
}
