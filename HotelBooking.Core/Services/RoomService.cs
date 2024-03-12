using System;
using System.Linq;
using HotelBooking.Core.Interfaces;

namespace HotelBooking.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> roomRepository;
        private readonly IRepository<Booking> bookingRepository;

        public RoomService(IRepository<Room> roomRepository, IRepository<Booking> bookingRepository)
        {
            this.roomRepository = roomRepository;
            this.bookingRepository = bookingRepository;
        }

        public int FindAvailableRoom(DateTime startDate, DateTime endDate)
        {

            if (startDate < DateTime.Now || endDate < startDate)
                return -1;
            
            var activeBookings = bookingRepository.GetAll().Where(b => b.IsActive);
            foreach (var room in roomRepository.GetAll())
            {
                var activeBookingsForCurrentRoom = activeBookings.Where(b => b.RoomId == room.Id);

                bool isAvailable = activeBookingsForCurrentRoom.All(b => startDate > b.EndDate || endDate < b.StartDate);
                if (isAvailable)
                {
                    return room.Id;
                }
            }
            return -1;
        }

        public int GetTotalNumberOfRooms()
        {
            return roomRepository.GetAll().Count();
        }
    }
}