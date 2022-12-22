﻿using System;
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
                .Join(_context.Space, r => r.SpaceID, s => s.ID, (r, s) => new { ReservationID = r.Id, SpaceID = r.SpaceID, Kenteken = r.Kenteken, CarID = r.CarID, ArrivalTime = r.ArrivalTime, DepartureTime = r.DepartureTime, UserID = r.UserID, Status = r.Status, SpaceNumber = s.Spot, SpaceRow = s.Row, SpaceFloor = s.Floor, GarageID = s.GarageID }).Where(r => r.UserID == id)
                .Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { GarageName = g.Name, ReservationID = s.ReservationID, SpaceID = s.SpaceID, Kenteken = s.Kenteken, CarID = s.CarID, ArrivalTime = s.ArrivalTime, DepartureTime = s.DepartureTime, UserID = s.UserID, Status = s.Status, SpaceNumber = s.SpaceNumber, SpaceRow = s.SpaceRow, SpaceFloor = s.SpaceFloor, GarageID = s.GarageID })
                .ToListAsync();
            if (reservations == null || reservations.Count() == 0)
            {
                Problem("No reservations");
            }

            return JsonConvert.SerializeObject(reservations);
        }

        [HttpGet("Admin/Reservation")]
        public async Task<string> GetReservationsByAdmin()
        {
            var reservations = await _context.Reservations.Join(_context.Car, r => r.CarID, c => c.Id, (r, c) => new { r.Id, SpaceID = r.SpaceID, CarID = r.CarID, ArrivalTime = r.ArrivalTime, DepartureTime = r.DepartureTime, UserID = c.UserID, Kenteken = c.Kenteken, Status = r.Status })
               .Join(_context.Space, r => r.SpaceID, s => s.ID, (r, s) => new { ReservationID = r.Id, SpaceID = r.SpaceID, Kenteken = r.Kenteken, CarID = r.CarID, ArrivalTime = r.ArrivalTime, DepartureTime = r.DepartureTime, UserID = r.UserID, Status = r.Status, SpaceNumber = s.Spot, SpaceRow = s.Row, SpaceFloor = s.Floor, GarageID = s.GarageID })
               .Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { GarageName = g.Name, ReservationID = s.ReservationID, SpaceID = s.SpaceID, Kenteken = s.Kenteken, CarID = s.CarID, ArrivalTime = s.ArrivalTime, DepartureTime = s.DepartureTime, UserID = s.UserID, Status = s.Status, SpaceNumber = s.SpaceNumber, SpaceRow = s.SpaceRow, SpaceFloor = s.SpaceFloor, GarageID = s.GarageID })
               .ToListAsync();

            if (reservations == null || reservations.Count() == 0)
            {
                Problem("No reservations");
            }

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
            var ReservationsByCar = _context.Reservations.Where(r => r.CarID == reservations.CarID).ToList();
            var garage = _context.Space.Where(s => s.ID == reservations.SpaceID).Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { g.Id, s.GarageID, g.OpeningTime, g.ClosingTime }).Where(s => s.GarageID == s.Id).First();
            var ArrivalTime = reservations.ArrivalTime.AddMinutes(-15);
            var DepartureTime = reservations.DepartureTime?.AddMinutes(15);

            if (ArrivalTime.TimeOfDay < garage.OpeningTime.Value.TimeOfDay || DepartureTime.Value.TimeOfDay > garage.ClosingTime.Value.TimeOfDay)
            {
                return BadRequest("Garage is closed");
            }

            foreach (var res in ReservationsByCar)
            {
                if (res.ArrivalTime <= ArrivalTime && res.ArrivalTime <= DepartureTime && res.DepartureTime >= ArrivalTime || res.ArrivalTime >= ArrivalTime && res.ArrivalTime <= DepartureTime)
                {
                    if (res.Id != reservations.Id)
                    {
                        return BadRequest("You already have a reservation for this license plate");
                    }
                }
            }

            if (id != reservations.Id)
            {
                return BadRequest("OOPS, something went wrong");
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
            #region PostReservations
            var ReservationsByCar = _context.Reservations.Where(r => r.CarID == reservations.CarID).ToList();
            var garage = _context.Space.Where(s => s.ID == reservations.SpaceID).Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { g.Id, s.GarageID, g.OpeningTime, g.ClosingTime }).Where(s => s.GarageID == s.Id).First();
            var ArrivalTime = reservations.ArrivalTime.AddMinutes(-15);
            var DepartureTime = reservations.DepartureTime?.AddMinutes(15);

            if (ArrivalTime.TimeOfDay < garage.OpeningTime.Value.TimeOfDay || DepartureTime.Value.TimeOfDay > garage.ClosingTime.Value.TimeOfDay)
            {
                return BadRequest("Garage is closed");
            }

            foreach (var res in ReservationsByCar)
            {
                if (res.ArrivalTime <= ArrivalTime && res.ArrivalTime <= DepartureTime && res.DepartureTime >= ArrivalTime || res.ArrivalTime >= ArrivalTime && res.ArrivalTime <= DepartureTime)
                {
                    return BadRequest("You already have a reservation for this license plate");
                }
            }

            _context.Reservations.Add(reservations);
            await _context.SaveChangesAsync();
            #endregion

            #region PostReceipt


            List<Pricing> pricing = _context.Pricing.Where(p => p.StartingTime.Value.TimeOfDay < reservations.ArrivalTime.TimeOfDay && p.EndingTime.Value.TimeOfDay < reservations.DepartureTime.Value.TimeOfDay && p.StartingTime.Value.DayOfWeek == reservations.ArrivalTime.DayOfWeek).ToList();

            if (pricing != null && pricing.Count() != 0)
            {
                decimal TotalPrice = 0;
                decimal normalPrice = _context.Space.Where(s => s.ID == reservations.SpaceID).Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { g.Id, s.GarageID, g.NormalPrice }).Where(s => s.GarageID == s.Id).First().NormalPrice;
                TimeSpan hours;
                TimeSpan hoursToRemove = new TimeSpan();

                foreach (var price in pricing)
                {
                    if (price.StartingTime.Value.TimeOfDay > reservations.ArrivalTime.TimeOfDay)
                    {
                        if (price.EndingTime.Value.TimeOfDay > reservations.DepartureTime.Value.TimeOfDay)
                        {
                            hours = reservations.DepartureTime.Value.TimeOfDay - price.StartingTime.Value.TimeOfDay;
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
                        if (price.EndingTime.Value.TimeOfDay > reservations.DepartureTime.Value.TimeOfDay)
                        {
                            hours = reservations.DepartureTime.Value.TimeOfDay - reservations.ArrivalTime.TimeOfDay;
                            hoursToRemove += hours;
                            decimal priceToAdd = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                            TotalPrice += priceToAdd;
                        }
                        else
                        {
                            hours = price.EndingTime.Value.TimeOfDay - reservations.ArrivalTime.TimeOfDay;
                            hoursToRemove += hours;
                            decimal priceToAdd = price.Price * (decimal)(hours.Hours + hours.Minutes / 60.0);
                            TotalPrice += priceToAdd;
                        }
                    }

                    List<Pricing> pricingOverlap = pricing.Where(p => p.StartingTime.Value.TimeOfDay < price.StartingTime.Value.TimeOfDay && p.EndingTime.Value.TimeOfDay < price.EndingTime.Value.TimeOfDay).ToList();

                    foreach (var pOverlap in pricingOverlap)
                    {

                    }
                }

                hours = reservations.DepartureTime.Value.TimeOfDay - reservations.ArrivalTime.TimeOfDay;
                if (hours > hoursToRemove)
                {
                    hours -= hoursToRemove;
                    decimal priceToAdd = normalPrice * (decimal)(hours.Hours + hours.Minutes / 60.0);
                    TotalPrice += priceToAdd;
                }

                TotalPrice = decimal.Parse(TotalPrice.ToString("0.00"));
                Receipt receipt = new Receipt { ReservationID = reservations.Id, Price = TotalPrice };

                _context.Receipt.Add(receipt);
                await _context.SaveChangesAsync();
            }
            else
            {
                TimeSpan hours = reservations.DepartureTime.Value - reservations.ArrivalTime;

                decimal normalPrice = _context.Space.Where(s => s.ID == reservations.SpaceID).Join(_context.Garage, s => s.GarageID, g => g.Id, (s, g) => new { g.Id, s.GarageID, g.NormalPrice }).Where(s => s.GarageID == s.Id).First().NormalPrice;

                decimal price = normalPrice * (decimal)(hours.Hours + hours.Minutes / 60.0);

                price = decimal.Parse(price.ToString("0.00"));

                Receipt receipt = new Receipt { ReservationID = reservations.Id, Price = price };

                _context.Receipt.Add(receipt);
                await _context.SaveChangesAsync();
            }
            
            #endregion

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
