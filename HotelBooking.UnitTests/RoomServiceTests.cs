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

        public static IEnumerable<object[]> GetDataForAvailableRooms()
        {
            
            var data = new List<object[]>()
            {
                new object[] {DateTime.Now.AddDays(2), DateTime.Now.AddDays(4), true, 1 }, // Room available for the period
                new object[] {DateTime.Now.AddDays(2), DateTime.Now.AddDays(4),  false, -1 }, // No room available for the period
                new object[] {DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2),  true, -1 }, // Room available for the period but is in the past
                new object[] {DateTime.Now.AddDays(-4), DateTime.Now.AddDays(-2),  false, -1 } // No room available for the period and is in the past
            };
            return data;
        }

        [Theory]
        [MemberData(nameof(GetDataForAvailableRooms))]
        public void FindAvailableRoom_ReturnsCorrectRoomId(DateTime startDate, DateTime endDate, bool isAvailable, int expectedRoomId)
        {
            // Arrange
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
