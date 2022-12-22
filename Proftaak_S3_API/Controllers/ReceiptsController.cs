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
    public class ReceiptsController : ControllerBase
    {
        private readonly ProftaakContext _context;

        public ReceiptsController(ProftaakContext context)
        {
            _context = context;
        }

        // GET: api/Receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceipt()
        {
            return await _context.Receipt.ToListAsync();
        }

        // GET: api/Receipts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Receipt>> GetReceipt(int id)
        {
            var receipt = await _context.Receipt.FindAsync(id);

            if (receipt == null)
            {
                return NotFound();
            }

            return receipt;
        }

        // GET: api/Receipts/byday/5
        [HttpGet("byday/{currentDay}")]
        public async Task<ActionResult<decimal>> GetReceiptsByDay(DateTime currentDay)
        {
            var currentDayReservations = _context.Reservations.Where(g => g.DepartureTime.Value.Date == currentDay).ToListAsync();
            var idListReservations = new List<int>();
            foreach (var reservation in currentDayReservations.Result)
            {
                idListReservations.Add(reservation.Id);
            }
            var receiptsByReservationId = await _context.Receipt.Where(r => idListReservations.Contains(r.ReservationID)).ToListAsync();
            decimal totalRevenue = 0;
            foreach (var receipt in receiptsByReservationId)
            {
                totalRevenue += receipt.Price;
            }

            return totalRevenue;
        }

        // PUT: api/Receipts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReceipt(int id, Receipt receipt)
        {
            if (id != receipt.ID)
            {
                return BadRequest("id`s not the same");
            }

            _context.Entry(receipt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiptExists(id))
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

        // POST: api/Receipts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Receipt>> PostReceipt(Receipt receipt)
        {
            _context.Receipt.Add(receipt);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReceipt", new { id = receipt.ID }, receipt);
        }

        // DELETE: api/Receipts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            var receipt = await _context.Receipt.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            _context.Receipt.Remove(receipt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReceiptExists(int id)
        {
            return _context.Receipt.Any(e => e.ID == id);
        }
    }
}
