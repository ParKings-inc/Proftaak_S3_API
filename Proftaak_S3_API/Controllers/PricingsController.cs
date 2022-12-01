using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingsController : ControllerBase
    {
        private readonly ProftaakContext _context;

        public PricingsController(ProftaakContext context)
        {
            _context = context;
        }

        // GET: api/Pricings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pricing>>> GetPricing()
        {
            return await _context.Pricing.ToListAsync();
        }

        // GET: api/Pricings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pricing>> GetPricing(int id)
        {
            var pricing = await _context.Pricing.FindAsync(id);

            if (pricing == null)
            {
                return NotFound();
            }

            return pricing;
        }

        // PUT: api/Pricings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPricing(int id, Pricing pricing)
        {
            if (id != pricing.ID)
            {
                return BadRequest("id`s not the same");
            }

            _context.Entry(pricing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PricingExists(id))
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

        // POST: api/Pricings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pricing>> PostPricing(Pricing pricing)
        {
            _context.Pricing.Add(pricing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPricing", new { id = pricing.ID }, pricing);
        }

        // DELETE: api/Pricings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePricing(int id)
        {
            var pricing = await _context.Pricing.FindAsync(id);
            if (pricing == null)
            {
                return NotFound();
            }

            _context.Pricing.Remove(pricing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PricingExists(int id)
        {
            return _context.Pricing.Any(e => e.ID == id);
        }

        [HttpGet("Day/{day}/{id}")]
        public async Task<ActionResult<IEnumerable<Pricing>>> GetPricesByDay(DateTime day, int id)
        {
            var d = day.Date;

            var pricing = await _context.Pricing.Where(p => p.GarageID == id).ToListAsync();
            List<Pricing> pricesToReturn = new List<Pricing>();

            foreach (var price in pricing)
            {
                if (price.StartingTime.Value.Date != d)
                {
                    pricesToReturn.Add(price);
                }
            }

            if (pricesToReturn == null)
            {
                return NotFound();
            }

            return pricesToReturn;
        }

        [HttpGet("Week/{day}/{id}")]
        public async Task<ActionResult<IEnumerable<Pricing>>> GetPricesByWeek(DateTime day, int id)
        {
            int currentDayOfWeek = (int)day.DayOfWeek;
            DateTime sunday = day.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();

            var pricing = await _context.Pricing.Where(p => p.GarageID == id).ToListAsync();
            List<Pricing> pricesToReturn = new List<Pricing>();

            foreach (var price in pricing)
            {
                for (int i = 0; i < dates.Count; i++)
                {
                    if (price.StartingTime.Value.Date == dates[i].Date)
                    {
                        pricesToReturn.Add(price);
                    }
                }
            }

            if (pricesToReturn == null)
            {
                return NotFound();
            }

            return pricesToReturn;
        }

        [HttpGet("Month/{day}/{id}")]
        public async Task<ActionResult<IEnumerable<Pricing>>> GetPricesByMonth(DateTime day, int id)
        {
            int currentDayOfWeek = (int)day.DayOfWeek;
            DateTime sunday = day.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            var dates = Enumerable.Range(0, 32).Select(days => monday.AddDays(days)).ToList();

            var pricing = await _context.Pricing.Where(p => p.GarageID == id).ToListAsync();
            List<Pricing> pricesToReturn = new List<Pricing>();

            foreach (var price in pricing)
            {
                for (int i = 0; i < dates.Count; i++)
                {
                    if (price.StartingTime.Value.Date == dates[i].Date)
                    {
                        pricesToReturn.Add(price);
                    }
                }
            }

            if (pricesToReturn == null)
            {
                return NotFound();
            }

            return pricesToReturn;
        }
    }
}
