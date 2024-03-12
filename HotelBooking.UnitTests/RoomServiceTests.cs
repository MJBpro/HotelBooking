using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using HotelBooking.Core;
using HotelBooking.Core.Services;

namespace HotelBooking.UnitTests
{
    public class RoomServiceTests
    {
        private readonly RoomService roomService;
        private readonly Mock<IRepository<Room>> mockRoomRepository;
        private readonly Mock<IRepository<Booking>> mockBookingRepository;

        public RoomServiceTests()
        {
            mockRoomRepository = new Mock<IRepository<Room>>();
            mockBookingRepository = new Mock<IRepository<Booking>>();
            roomService = new RoomService(mockRoomRepository.Object, mockBookingRepository.Object);
        }

        [Theory]
        [InlineData("2023-04-01", "2023-04-05", true, 1)]  // Room available for the period
        [InlineData("2023-04-01", "2023-04-05", false, -1)] // No room available for the period
        public void FindAvailableRoom_ReturnsCorrectRoomId(string start, string end, bool isAvailable, int expectedRoomId)
        {
            // Arrange
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = DateTime.Parse(end);
            var rooms = new List<Room> { new Room { Id = 1 } };
            var bookings = isAvailable ? new List<Booking>() : new List<Booking>
            {
                new Booking { RoomId = 1, StartDate = startDate, EndDate = endDate, IsActive = true }
            };

            mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);
            mockBookingRepository.Setup(b => b.GetAll()).Returns(bookings);

            // Act
            var roomId = roomService.FindAvailableRoom(startDate, endDate);

            // Assert
            Assert.Equal(expectedRoomId, roomId);
        }

        [Theory]
        [InlineData(0)]  // No rooms available
        [InlineData(5)]  // Five rooms available
        public void GetTotalNumberOfRooms_ReturnsCorrectNumber(int numberOfRooms)
        {
            // Arrange
            var rooms = new List<Room>();
            for (var i = 0; i < numberOfRooms; i++)
            {
                rooms.Add(new Room { Id = i });
            }
            mockRoomRepository.Setup(r => r.GetAll()).Returns(rooms);

            // Act
            var totalRooms = roomService.GetTotalNumberOfRooms();

            // Assert
            Assert.Equal(numberOfRooms, totalRooms);
        }
    }
}
