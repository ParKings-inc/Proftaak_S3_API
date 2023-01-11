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
using Microsoft.AspNetCore.SignalR;
using Proftaak_S3_API.Hubs;
using Proftaak_S3_API.Hubs.Clients;

namespace Proftaak_S3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly ProftaakContext _context;
        private readonly IHubContext<RevenueHub, IRevenueClient> _revenueHub;

        public ReceiptsController(ProftaakContext context, IHubContext<RevenueHub, IRevenueClient> revenueHub)
        {
            _context = context;
            _revenueHub = revenueHub;
        }
        // GET: api/Receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetReceipt()
        {
            return await _context.Receipt.ToListAsync();
        }
        // GET: api/Receipts/5
        [HttpGet("{id}")]
        public async Task<string> GetReceipt(int id)
        {
            var receipt = await _context.Receipt.Join(_context.Reservations, rec => rec.ReservationID, r => r.Id, (rec, r) => new { rec.ID, rec.Price, r.ArrivalTime, r.DepartureTime }).Where(rec => rec.ID == id).FirstAsync();

            if (receipt == null)
            {
                Problem("No receipt");
            }

            return JsonConvert.SerializeObject(receipt);
        }

        [HttpGet("User/{id}")]
        public async Task<string> GetReceiptByUser(string id)
        {
            var receipts = await _context.Receipt.Join(_context.Reservations, rec => rec.ReservationID, r => r.Id, (rec, r) => new { rec.ID, r.SpaceID, r.CarID, rec.Price, r.ArrivalTime, r.DepartureTime }).Join(_context.Car, r => r.CarID, s => s.Id, (r, s) => new { r.ID, r.SpaceID, r.CarID, s.UserID, r.Price, r.ArrivalTime, r.DepartureTime }).Where(r => r.UserID == id).ToListAsync();

            if (receipts == null || receipts.Count() == 0)
            {
                Problem("No receipts");
            }

            return JsonConvert.SerializeObject(receipts);
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

            await _revenueHub.Clients.All.ReceiveRevenue(totalRevenue);

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
