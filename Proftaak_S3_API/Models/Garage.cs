using Microsoft.EntityFrameworkCore;

namespace Proftaak_S3_API.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? OpeningTime { get; set; }
        public DateTime? ClosingTime { get; set; }
        public int MaxSpace { get; set; }
        [Precision(9, 2)]
        public decimal NormalPrice { get; set; }
        [Precision(9, 2)]
        public decimal MaxPrice { get; set; }
    }
}
