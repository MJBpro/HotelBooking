using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core.Interfaces;

namespace HotelBooking.Core.Services
{
    public class BookingManager : IBookingManager
    {
        private readonly IRepository<Booking> bookingRepository;
        private readonly IRoomService roomService;

        public BookingManager(IRepository<Booking> bookingRepository, IRoomService roomService)
        {
            this.bookingRepository = bookingRepository;
            this.roomService = roomService;
        }

        public bool CreateBooking(Booking booking)
        {
            if (booking == null)
            {
                throw new ArgumentNullException(nameof(booking), "Booking cannot be null.");
            }

            if (booking.StartDate >= booking.EndDate)
            {
                throw new ArgumentException("EndDate must be greater than StartDate", nameof(booking));
            }

            var roomId = roomService.FindAvailableRoom(booking.StartDate, booking.EndDate);
            if (roomId < 0) return false;
            booking.RoomId = roomId;
            booking.IsActive = true;
            bookingRepository.Add(booking);
            return true;
        }
        public List<DateTime> GetFullyOccupiedDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("The start date cannot be later than the end date.");

            List<DateTime> fullyOccupiedDates = new List<DateTime>();
            int noOfRooms = roomService.GetTotalNumberOfRooms();
            var bookings = bookingRepository.GetAll();

            if (bookings.Any())
            {
                for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
                {
                    var noOfBookings = from b in bookings
                        where b.IsActive && d >= b.StartDate && d <= b.EndDate
                        select b;
                    if (noOfBookings.Count() >= noOfRooms)
                        fullyOccupiedDates.Add(d);
                }
            }
            return fullyOccupiedDates;
        }
    }
    
}