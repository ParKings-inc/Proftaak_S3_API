using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Proftaak_S3_API.Hubs;
using Proftaak_S3_API.Hubs.Clients;
using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GaragesController : ControllerBase
    {
        private readonly ProftaakContext _context;
        private readonly IHubContext<SpaceHub, ISpaceClient> _spaceHub;
        private readonly IHubContext<ReservationHub, IReservationClient> _reservationHub;

        public GaragesController(ProftaakContext context, IHubContext<SpaceHub, ISpaceClient> spaceHub, IHubContext<ReservationHub, IReservationClient> reservationHub)
        {
            _context = context;
            _spaceHub = spaceHub;
            _reservationHub = reservationHub;   
        }

        // GET: api/Garages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Garage>>> GetGarage()
        {
          if (_context.Garage == null)
          {
              return NotFound();
          }
            return await _context.Garage.ToListAsync();
        }

        // GET: api/Garages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Garage>> GetGarage(int id)
        {
            if (_context.Garage == null)
            {
                return NotFound();
            }
            var garage = await _context.Garage.FindAsync(id);

            if (garage == null)
            {
                return NotFound();
            }

            return garage;
        }

        // PUT: api/Garages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGarage(int id, Garage garage)
        {
            if (id != garage.Id)
            {
                return BadRequest("id`s not the same");
            }

            _context.Entry(garage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GarageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Garages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Garage>> PostGarage(Garage garage)
        {
          if (_context.Garage == null)
          {
              return Problem("Entity set 'ProftaakContext.Garage'  is null.");
          }
            _context.Garage.Add(garage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGarage", new { id = garage.Id }, garage);
        }

        // DELETE: api/Garages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGarage(int id)
        {
            if (_context.Garage == null)
            {
                return NotFound();
            }
            var garage = await _context.Garage.FindAsync(id);
            if (garage == null)
            {
                return NotFound();
            }

            _context.Garage.Remove(garage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("EnterGarage/{garageId}/{licencePlate}")]
        public async Task<bool> EnterGarage(int garageId, string licencePlate) {
            Car? car = await _context.Car!.FirstOrDefaultAsync(i => i.Kenteken == licencePlate);
            if (car == null) {
                return false;
            }

            Reservations? reservation = await _context.GetReservation(licencePlate, garageId, true);
            if (reservation == null) {
                return await TryCreateNewReservation(garageId, car);
            }
          
            return await TryEnterWithExistingReservation(garageId, reservation, car);
        }

        [HttpPut("LeaveGarage/{garageId}/{licencePlate}")]
        public async Task<bool> LeaveGarage(int garageId, string licencePlate) {
            Reservations? reservation = await _context.GetReservation(licencePlate, garageId, false);
            if (reservation == null) {
                return false;
            }

            reservation.DepartureTime = DateTime.Now;
            reservation.Status = "Awaiting payment";
            await _context.SaveChangesAsync();

            #region PostReceipt

            List<Pricing> pricing = new List<Pricing>();
            try
            {
                List<Pricing> p;
                p = _context.Pricing.ToList();
                foreach (var price in p)
                {
                    if (price.StartingTime.Value.TimeOfDay < reservation.ArrivalTime.TimeOfDay || price.EndingTime.Value.TimeOfDay < reservation.DepartureTime.Value.TimeOfDay && price.StartingTime.Value.DayOfWeek == reservation.ArrivalTime.DayOfWeek)
                    {
                        pricing.Add(price);
                    }

                }
            }
            catch (Exception)
            {
                pricing = null;
            }

            if (pricing != null && pricing.Count() != 0)
            {
                decimal TotalPrice = 0;
                decimal normalPrice = _context.Space.Where(s => s.ID == reservation.SpaceID).Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { g.Id, s.GarageID, g.NormalPrice }).Where(s => s.GarageID == s.Id).First().NormalPrice;
                TimeSpan hours;
                TimeSpan hoursToRemove = new TimeSpan();

                foreach (var price in pricing)
                {
                    if (price.StartingTime.Value.TimeOfDay > reservation.ArrivalTime.TimeOfDay)
                    {
                        if (price.EndingTime.Value.TimeOfDay > reservation.DepartureTime.Value.TimeOfDay)
                        {
                            hours = reservation.DepartureTime.Value.TimeOfDay - price.StartingTime.Value.TimeOfDay;
                            hoursToRemove += hours;
                            decimal priceToAdd = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                            TotalPrice += priceToAdd;
                        }
                        else
                        {
                            hours = price.EndingTime.Value.TimeOfDay - price.StartingTime.Value.TimeOfDay;
                            hoursToRemove += hours;
                            decimal priceToAdd = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                            TotalPrice += priceToAdd;
                        }
                    }
                    else
                    {
                        if (price.EndingTime.Value.TimeOfDay > reservation.DepartureTime.Value.TimeOfDay)
                        {
                            hours = reservation.DepartureTime.Value.TimeOfDay - reservation.ArrivalTime.TimeOfDay;
                            hoursToRemove += hours;
                            decimal priceToAdd = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                            TotalPrice += priceToAdd;
                        }
                        else
                        {
                            hours = price.EndingTime.Value.TimeOfDay - reservation.ArrivalTime.TimeOfDay;
                            hoursToRemove += hours;
                            decimal priceToAdd = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                            TotalPrice += priceToAdd;
                        }
                    }

                    List<Pricing> pricingOverlap = pricing.Where(p => p.StartingTime.Value.TimeOfDay < price.StartingTime.Value.TimeOfDay || p.EndingTime.Value.TimeOfDay < price.EndingTime.Value.TimeOfDay).ToList();

                    foreach (var pOverlap in pricingOverlap)
                    {
                        if (pOverlap.recurring != true)
                        {
                            foreach (var p in pricingOverlap)
                            {
                                if (p.StartingTime.Value.TimeOfDay <= pOverlap.StartingTime.Value.TimeOfDay && p.EndingTime.Value.TimeOfDay <= pOverlap.EndingTime.Value.TimeOfDay)
                                {
                                    if (pOverlap.StartingTime.Value.TimeOfDay >= p.StartingTime.Value.TimeOfDay)
                                    {
                                        if (pOverlap.EndingTime.Value.TimeOfDay >= p.EndingTime.Value.TimeOfDay)
                                        {
                                            hours = p.EndingTime.Value.TimeOfDay - pOverlap.StartingTime.Value.TimeOfDay;
                                            hoursToRemove += hours;
                                            decimal priceToRemove = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                                            TotalPrice -= priceToRemove;
                                        }
                                        else
                                        {
                                            hours = pOverlap.EndingTime.Value.TimeOfDay - pOverlap.StartingTime.Value.TimeOfDay;
                                            hoursToRemove += hours;
                                            decimal priceToRemove = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                                            TotalPrice -= priceToRemove;
                                        }
                                    }
                                    else
                                    {
                                        if (pOverlap.EndingTime.Value.TimeOfDay >= p.EndingTime.Value.TimeOfDay)
                                        {
                                            hours = p.EndingTime.Value.TimeOfDay - p.StartingTime.Value.TimeOfDay;
                                            hoursToRemove += hours;
                                            decimal priceToRemove = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                                            TotalPrice -= priceToRemove;
                                        }
                                        else
                                        {
                                            hours = pOverlap.EndingTime.Value.TimeOfDay - p.StartingTime.Value.TimeOfDay;
                                            hoursToRemove += hours;
                                            decimal priceToRemove = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                                            TotalPrice -= priceToRemove;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                hours = reservation.DepartureTime.Value.TimeOfDay - reservation.ArrivalTime.TimeOfDay;
                if (hours > hoursToRemove)
                {
                    hours -= hoursToRemove;
                    decimal priceToAdd = normalPrice * (decimal)(hours.Hours + hours.Minutes / 60.0);
                    TotalPrice += priceToAdd;
                }

                TotalPrice = decimal.Parse(TotalPrice.ToString("0.00"));
                if(TotalPrice == 0)
                {
                    TotalPrice= 0.01M;
                }
                Receipt receipt = new Receipt { ReservationID = reservation.Id, Price = TotalPrice };

                _context.Receipt.Add(receipt);
                await _context.SaveChangesAsync();
            }
            else
            {
                TimeSpan hours = reservation.DepartureTime.Value - reservation.ArrivalTime;

                decimal normalPrice = _context.Space.Where(s => s.ID == reservation.SpaceID).Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { g.Id, s.GarageID, g.NormalPrice }).Where(s => s.GarageID == s.Id).First().NormalPrice;

                decimal price = normalPrice * (decimal)(hours.Hours + hours.Minutes / 60.0);

                price = decimal.Parse(price.ToString("0.00"));

                Receipt receipt = new Receipt { ReservationID = reservation.Id, Price = price };

                _context.Receipt.Add(receipt);
                await _context.SaveChangesAsync();
            }

            #endregion
            Space space = await _context.Space.FindAsync(reservation.SpaceID);
            space.StatusId = 1;
            _context.Entry(space).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var spaces = await _context.Space.Where(i => i.GarageID == garageId).ToListAsync();
            await _spaceHub.Clients.All.ReceiveSpaces(spaces);

            var reservations = await _context.Reservations.ToListAsync();
            await _reservationHub.Clients.All.ReceiveReservation(reservations);
            return true;
        }

        private async Task<bool> TryEnterWithExistingReservation(int garageId, Reservations reservation, Car car) {
            if (reservation.Status != "Accepted") {
                return await TryCreateNewReservation(garageId, car);
            } else
            {
                reservation.ArrivalTime = DateTime.Now;
                _context.Entry(reservation).State = EntityState.Modified;
                Space space = await _context.Space.FindAsync(reservation.SpaceID);
                space.StatusId = 2;
                _context.Entry(space).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch 
                {
                }
            }
            var spaces = await _context.Space.Where(i => i.GarageID == garageId).ToListAsync();
            await _spaceHub.Clients.All.ReceiveSpaces(spaces);

            var reservations = await _context.Reservations.ToListAsync();
            await _reservationHub.Clients.All.ReceiveReservation(reservations);
            return true;
        }

        private async Task<bool> TryCreateNewReservation(int garageId, Car car) {
            Garage? garage = await _context.Garage!.FirstOrDefaultAsync(i => i.Id == garageId);
            if (garage == null) {
                return false;
            }
            DateTime arrival = DateTime.Now;
            if (garage.GetAvailableSpaceCount(_context, arrival, null) <= 0) {
                return false;
            }
            await CreateReservation(garage, car, arrival);
            var spaces = await _context.Space.Where(i => i.GarageID == garageId).ToListAsync();
            await _spaceHub.Clients.All.ReceiveSpaces(spaces);

            var reservations = await _context.Reservations.ToListAsync();
            await _reservationHub.Clients.All.ReceiveReservation(reservations);
            return true;
        }

        private async Task<bool> CreateReservation(Garage garage, Car car, DateTime arrival) {
            int spaceId = await garage.GetNextAvailableSpaceId(_context, arrival, null);
            if (spaceId == -1) {
                return false;
            }

            Reservations reservation = new Reservations() {
                SpaceID = spaceId,
                CarID = car.Id,
                Status = "Accepted",
                ArrivalTime = DateTime.Now,
                DepartureTime = null
            };
            _context.Reservations!.Add(reservation);
           Space space = await _context.Space.FindAsync(spaceId);
            space.StatusId = 2;
             _context.Entry(space).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        private bool GarageExists(int id)
        {
            return (_context.Garage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
