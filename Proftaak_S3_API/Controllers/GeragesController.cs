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
    public class GeragesController : ControllerBase
    {
        private readonly ProftaakContext _context;

        public GeragesController(ProftaakContext context)
        {
            _context = context;
        }

        // GET: api/Gerages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gerage>>> GetGerage()
        {
          if (_context.Gerage == null)
          {
              return NotFound();
          }
            return await _context.Gerage.ToListAsync();
        }

        // GET: api/Gerages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gerage>> GetGerage(int id)
        {
          if (_context.Gerage == null)
          {
              return NotFound();
          }
            var gerage = await _context.Gerage.FindAsync(id);

            if (gerage == null)
            {
                return NotFound();
            }

            return gerage;
        }

        // PUT: api/Gerages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGerage(int id, Gerage gerage)
        {
            if (id != gerage.Id)
            {
                return BadRequest();
            }

            _context.Entry(gerage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GerageExists(id))
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

        // POST: api/Gerages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gerage>> PostGerage(Gerage gerage)
        {
          if (_context.Gerage == null)
          {
              return Problem("Entity set 'ProftaakContext.Gerage'  is null.");
          }
            _context.Gerage.Add(gerage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGerage", new { id = gerage.Id }, gerage);
        }

        // DELETE: api/Gerages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGerage(int id)
        {
            if (_context.Gerage == null)
            {
                return NotFound();
            }
            var gerage = await _context.Gerage.FindAsync(id);
            if (gerage == null)
            {
                return NotFound();
            }

            _context.Gerage.Remove(gerage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GerageExists(int id)
        {
            return (_context.Gerage?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
