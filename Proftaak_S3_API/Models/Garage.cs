using Microsoft.EntityFrameworkCore;

namespace Proftaak_S3_API.Models {
    public class Garage {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime? OpeningTime { get; set; }

        public DateTime? ClosingTime { get; set; }
        public int MaxSpace { get; set; }
        [Precision(9, 2)]
        public decimal NormalPrice { get; set; }
        [Precision(9, 2)]
        public decimal MaxPrice { get; set; }

        public int GetAvailableSpaceCount(ProftaakContext context, DateTime arrival, DateTime? departure) {
            if (OpeningTime != null && arrival.TimeOfDay < OpeningTime.Value.TimeOfDay) {
                return 0;
            }

            if (ClosingTime != null && departure != null && departure.Value.TimeOfDay > ClosingTime.Value.TimeOfDay) {
                return 0;
            }

            int occupiedSpaceCount = (from s in context.Reservations
                                      join sa in context.Space! on s.SpaceID equals sa.ID
                                      where sa.GarageID == Id &&
                                      ((arrival <= s.DepartureTime && arrival >= s.ArrivalTime) ||
                                       (departure <= s.DepartureTime && departure >= s.DepartureTime))
                                      select sa).Count();

            return MaxSpace - occupiedSpaceCount;
        }

        /// <summary>
        /// Gets the next available space ID.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="arrival">The time of arrival</param>
        /// <param name="departure">The time of departure. May be null.</param>
        /// <returns>The next available space ID.</returns>
        public async Task<int> GetNextAvailableSpaceId(ProftaakContext context, DateTime arrival, DateTime? departure) {
            if (OpeningTime != null && arrival.TimeOfDay < OpeningTime.Value.TimeOfDay) {
                return -1;
            }

            if (ClosingTime != null && departure != null && departure.Value.TimeOfDay > ClosingTime.Value.TimeOfDay) {
                return -1;
            }

            List<int> allIds = await (from s in context.Space
                                      where s.GarageID == Id
                                      select s.ID).ToListAsync();
            List<int> usedIds = await (from s in context.Space
                                       where s.GarageID == Id
                                       join r in context.Reservations! on s.ID equals r.SpaceID
                                       where (arrival <= r.DepartureTime && arrival >= r.ArrivalTime) ||
                                             r.DepartureTime == null ||
                                             (departure <= r.DepartureTime && departure >= r.DepartureTime)
                                       select s.ID).ToListAsync();
            return allIds.Where(i => !usedIds.Contains(i)).FirstOrDefault(-1);
        }
    }
}
