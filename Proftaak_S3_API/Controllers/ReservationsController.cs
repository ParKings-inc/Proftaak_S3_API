using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ProftaakContext _context;

        public ReservationsController(ProftaakContext context)
        {
            _context = context;
        }

        // GET: api/Reservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservations>>> GetReservations()
        {
            return await _context.Reservations.ToListAsync();
        }

        [HttpGet("User/{id}")]
        public async Task<string> GetReservationsByUser(string id)
        {
            var reservations = await _context.Reservations.Join(_context.Car, r => r.CarID, c => c.Id, (r, c) => new { r.Id, SpaceID = r.SpaceID, CarID = r.CarID, ArrivalTime = r.ArrivalTime, DepartureTime = r.DepartureTime, UserID = c.UserID, Kenteken = c.Kenteken, Status = r.Status })
                .Join(_context.Space, r => r.SpaceID, s => s.ID, (r, s) => new { ReservationID = r.Id, SpaceID = r.SpaceID, Kenteken = r.Kenteken, CarID = r.CarID, ArrivalTime = r.ArrivalTime, DepartureTime = r.DepartureTime, UserID = r.UserID, Status = r.Status, SpaceNumber = s.Spot, SpaceRow = s.Row, SpaceFloor = s.Floor }).Where(r => r.UserID == id).ToListAsync();

            return JsonConvert.SerializeObject(reservations);
        }


        // GET: api/Reservations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reservations>> GetReservations(int id)
        {
            var reservations = await _context.Reservations.FindAsync(id);

            if (reservations == null)
            {
                return NotFound();
            }

            return reservations;
        }

        // PUT: api/Reservations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservations(int id, Reservations reservations)
        {
            if (id != reservations.Id)
            {
                return BadRequest();
            }

            _context.Entry(reservations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationsExists(id))
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

        // POST: api/Reservations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Reservations>> PostReservations(Reservations reservations)
        {
            _context.Reservations.Add(reservations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservations", new { id = reservations.Id }, reservations);
        }

        // DELETE: api/Reservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservations(int id)
        {
            var reservations = await _context.Reservations.FindAsync(id);
            if (reservations == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservations);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReservationsExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
