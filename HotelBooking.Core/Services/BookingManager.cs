using System;
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
    }
}