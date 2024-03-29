﻿using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HotelBooking.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingsController(IRepository<Booking> bookingRepos, IRepository<Room> roomRepos,
            IRepository<Customer> customerRepos, IBookingManager manager)
        : Controller
    {
        private IRepository<Customer> customerRepository = customerRepos;
        private IRepository<Room> roomRepository = roomRepos;

        // GET: bookings
        [HttpGet(Name = "GetBookings")]
        public IEnumerable<Booking> Get()
        {
            return bookingRepos.GetAll();
        }

        // GET bookings/5
        [HttpGet("{id}", Name = "GetBooking")]
        public IActionResult Get(int id)
        {
            var item = bookingRepos.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST bookings
        [HttpPost]
        public IActionResult Post([FromBody]Booking booking)
        {
            if (booking == null)
            {
                return BadRequest();
            }

            bool created = manager.CreateBooking(booking);

            if (created)
            {
                return CreatedAtRoute("GetBookings", null);
            }
            else
            {
                return Conflict("The booking could not be created. All rooms are occupied. Please try another period.");
            }

        }

        // PUT bookings/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Booking booking)
        {
            if (booking == null || booking.Id != id)
            {
                return BadRequest();
            }

            var modifiedBooking = bookingRepos.Get(id);

            if (modifiedBooking == null)
            {
                return NotFound();
            }

            // This implementation will only modify the booking's state and customer.
            // It is not safe to directly modify StartDate, EndDate and Room, because
            // it could conflict with other active bookings.
            modifiedBooking.IsActive = booking.IsActive;
            modifiedBooking.CustomerId = booking.CustomerId;

            bookingRepos.Edit(modifiedBooking);
            return NoContent();
        }

        // DELETE bookings/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (bookingRepos.Get(id) == null)
            {
                return NotFound();
            }

            bookingRepos.Remove(id);
            return NoContent();
        }

    }
}
