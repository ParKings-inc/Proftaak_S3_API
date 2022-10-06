using Microsoft.EntityFrameworkCore;
namespace Proftaak_S3_API.Models
{
    public class ProftaakContext : DbContext
    {
        public ProftaakContext(DbContextOptions<ProftaakContext> options)
            : base(options)
        {
        }

        public DbSet<User>? User { get; set; }

        public DbSet<Car>? Car { get; set; }
        public DbSet<Garage>? Garage { get; set; }
        public DbSet<Reservations>? Reservations { get; set; }
        public DbSet<Space>? Space { get; set; }
        public DbSet<Receipt>? Receipt { get; set; }
        public DbSet<Pricing>? Pricing { get; set; }
    }
}
