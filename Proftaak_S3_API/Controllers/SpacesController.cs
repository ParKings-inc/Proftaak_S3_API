using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class SpacesController : ControllerBase
    {
        private readonly ProftaakContext _context;

        public SpacesController(ProftaakContext context)
        {
            _context = context;
        }


        [HttpGet("FreeSpaces/{id}")]
        public async Task<string> GetFreeSpaces(int id)
        {
            Garage garage = _context.Garage.Where(g => g.Id == id).FirstOrDefault();
            // var usedSpaces = await _context.Reservations.Where(s =>  s.SpaceID == id).ToListAsync();
            if (garage != null)
            {
                var Spaces = await _context.Space.ToListAsync();
                int count = 0;
                var time = garage.ClosingTime.ToString();
                var spaces = (from s in _context.Reservations
                              join sa in _context.Space on s.SpaceID equals sa.ID
                              where sa.GarageID == id && ((s.ArrivalTime.Date == DateTime.Now.Date && s.DepartureTime.HasValue == false) ||
                              (s.ArrivalTime.Date <=DateTime.Now.Date && s.DepartureTime.HasValue == true &&s.DepartureTime.Value >= DateTime.Now.Date))
                              select new { s.SpaceID }).Distinct();
                var allSpaces = await _context.Space.Where(s => s.GarageID == id).ToListAsync();

                var totalSpaces = await _context.Garage.FindAsync(id);

                List<int> ints = new List<int>();
                ints.Add(spaces.ToList().Count);
                ints.Add(allSpaces.Count);
                return JsonConvert.SerializeObject(ints);
            }
            return "No garage";
        }

        [HttpGet("reservations/create/getavailableSpace/{arrivalTime}/{endTime}/{garageId}")]
        public async Task<string> GetAvailableSpaces(DateTime arrivalTime, DateTime endTime,int garageId)
        {
            Garage garage = _context.Garage.Where(g => g.Id == garageId).FirstOrDefault();
            if (garage != null)
            {
                if(arrivalTime.TimeOfDay > garage.OpeningTime.GetValueOrDefault().TimeOfDay && endTime.TimeOfDay< garage.ClosingTime.GetValueOrDefault().TimeOfDay)
                {
                    var notAvailablespaces = (from s in _context.Reservations
                                              join sa in _context.Space on s.SpaceID equals sa.ID
                                              where sa.GarageID == garageId &&
                                              ((s.ArrivalTime <= arrivalTime && s.ArrivalTime <= endTime && s.DepartureTime >= arrivalTime) ||
                                              (s.ArrivalTime >= arrivalTime && s.ArrivalTime <= endTime))
                                              select sa.ID).Distinct().ToArray();

                    var availableSpaces = _context.Space.Where((s) => !notAvailablespaces.Contains(s.ID));

                    return JsonConvert.SerializeObject(availableSpaces);
                }
            }
            return "No garage";
        }


        // GET: api/Spaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Space>>> GetSpace()
        {
            return await _context.Space.ToListAsync();
        }

        // GET: api/Spaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Space>> GetSpace(int id)
        {
            var space = await _context.Space.FindAsync(id);

            if (space == null)
            {
                return NotFound();
            }

            return space;
        }

        // PUT: api/Spaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpace(int id, Space space)
        {
            if (id != space.ID)
            {
                return BadRequest("id`s not the same");
            }

            _context.Entry(space).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpaceExists(id))
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

        // POST: api/Spaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Space>> PostSpace(Space space)
        {
            _context.Space.Add(space);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpace", new { id = space.ID }, space);
        }

        // DELETE: api/Spaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpace(int id)
        {
            var space = await _context.Space.FindAsync(id);
            if (space == null)
            {
                return NotFound();
            }

            _context.Space.Remove(space);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpaceExists(int id)
        {
            return _context.Space.Any(e => e.ID == id);
        }
    }
}
