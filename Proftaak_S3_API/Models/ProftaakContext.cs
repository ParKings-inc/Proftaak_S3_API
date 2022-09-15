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

        public DbSet<Auto>? Auto { get; set; }
        public DbSet<Gerage>? Gerage { get; set; }
        public DbSet<Parking>? Parking { get; set; }
    }
}
