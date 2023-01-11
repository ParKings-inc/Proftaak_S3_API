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
        public DbSet<Role>? Role { get; set; }
        public DbSet<SpaceType>? SpaceType { get; set; }
        public DbSet<SpaceStatus>? SpaceStatus { get; set; }

        public async Task<Reservations?> GetReservation(string licencePlate, int garageId, bool checkTime) {
            Car? car = await Car!.FirstOrDefaultAsync(i => i.Kenteken == licencePlate);
            if (car == null) {
                return null;
            }
            List<Space> spaces = Space!.Where(s => s.GarageID == garageId).ToList();
            List<Reservations> reservations = await Reservations!.Where(i => i.CarID == car.Id).ToListAsync();
            if (checkTime) {
                reservations = reservations.Where(i => i.DepartureTime == null || i.DepartureTime > DateTime.Now).ToList();
            }
            return reservations.LastOrDefault(i => spaces.Any(s => s.ID == i.SpaceID));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpaceStatus>().HasData(new SpaceStatus { Id = 1, Name = "Available" }, new SpaceStatus { Id = 2, Name = "Occupied" }, new SpaceStatus { Id = 3, Name = "Unavailable" });
            modelBuilder.Entity<Garage>().HasData(new Garage { Id = 1, Name = "Test", MaxSpace = 5, MaxPrice = 5 });
            modelBuilder.Entity<Space>().HasData(
                new Space { ID = 1, GarageID = 1, Floor = 1, Row = "a", Spot = 1, TypeId = 1 },
                new Space { ID = 2, GarageID = 1, Floor = 1, Row = "a", Spot = 2, TypeId = 1 },
                new Space { ID = 3, GarageID = 1, Floor = 1, Row = "a", Spot = 3, TypeId = 1 },
                new Space { ID = 4, GarageID = 1, Floor = 1, Row = "b", Spot = 1, TypeId = 1 },
                new Space { ID = 5, GarageID = 1, Floor = 2, Row = "a", Spot = 1, TypeId = 1 }
                );
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "User" }, new Role { Id = 2, Name = "Admin"});
            modelBuilder.Entity<SpaceType>().HasData(new SpaceType { Id = 1, Name = "Normal" }, new SpaceType { Id = 2, Name = "Electric" });
        }
    }
}
