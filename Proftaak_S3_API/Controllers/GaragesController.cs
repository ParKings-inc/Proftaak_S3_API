using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GaragesController : ControllerBase
    {
        private readonly ProftaakContext _context;

        public GaragesController(ProftaakContext context)
        {
            _context = context;
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
            return true;
        }

        private async Task<bool> TryEnterWithExistingReservation(int garageId, Reservations reservation, Car car) {
            if (reservation.Status != "Accepted") {
                return await TryCreateNewReservation(garageId, car);
            } else
            {
                reservation.ArrivalTime = DateTime.Now;
                _context.Entry(reservation).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch 
                {
                }
            }

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
            await _context.SaveChangesAsync();
            return true;
        }

        private bool GarageExists(int id)
        {
            return (_context.Garage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
