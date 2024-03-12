using System;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Core;
using HotelBooking.Core.Interfaces;
using HotelBooking.Core.Services;
using HotelBooking.Infrastructure;
using HotelBooking.Infrastructure.Repositories;
using Moq;

namespace HotelBooking.IntegrationTests
{
    public class BookingManagerTests : IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly HotelBookingContext dbContext;
        private readonly BookingManager bookingManager;
        private readonly Mock<IRoomService> mockRoomService;

        public BookingManagerTests()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<HotelBookingContext>()
                            .UseSqlite(connection)
                            .Options;
            dbContext = new HotelBookingContext(options);

            IDbInitializer dbInitializer = new DbInitializer();
            if (dbInitializer == null) throw new ArgumentNullException(nameof(dbInitializer));
            dbInitializer.Initialize(dbContext);

            mockRoomService = new Mock<IRoomService>();

            var bookingRepo = new BookingRepository(dbContext);
            bookingManager = new BookingManager(bookingRepo, mockRoomService.Object);
        }

        [Theory]
        [InlineData("2023-04-01", "2023-04-05", true)] // Room available, booking should succeed
        [InlineData("2023-04-01", "2023-04-05", false)] // Room not available, booking should fail
        public void CreateBooking_ShouldSucceedOrFailBasedOnRoomAvailability(DateTime startDate, DateTime endDate, bool isRoomAvailable)
        {
            // Arrange
            var expectedRoomId = isRoomAvailable ? 1 : -1; 
            mockRoomService.Setup(service => service.FindAvailableRoom(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(expectedRoomId);

            var booking = new Booking
            {
                StartDate = startDate,
                EndDate = endDate,
                CustomerId = 1, 
                IsActive = true
            };

            // Act
            var result = bookingManager.CreateBooking(booking);

            // Assert
            Assert.Equal(isRoomAvailable, result);
        }

        public void Dispose()
        {
            dbContext.Dispose();
            connection.Close();
        }
    }
}
