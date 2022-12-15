namespace Proftaak_S3_API.Models
{
    public class Space
    {
        public int ID { get; set; }
        public int GarageID { get; set; }
        public int Floor { get; set; }
        public string? Row { get; set; }
        public int Spot { get; set; }
        public int TypeId { get; set; }
        public string? Status { get; set; }
    }
}