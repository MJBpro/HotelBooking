using System;
using HotelBooking.Core;
using HotelBooking.Core.Interfaces;
using HotelBooking.Core.Services;
using Xunit;
using Moq;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private readonly BookingManager bookingManager;
        private readonly Mock<IRepository<Booking>> mockBookingRepository;
        private readonly Mock<IRoomService> mockRoomService;

        public BookingManagerTests()
        {
            mockBookingRepository = new Mock<IRepository<Booking>>();
            mockRoomService = new Mock<IRoomService>();
            bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomService.Object);
        }

        [Theory]
        [InlineData(1, true)] // Indicates room is available, booking should succeed
        [InlineData(-1, false)] // Indicates no room available, booking should fail
        public void CreateBooking_AttemptsToCreateBookingBasedOnRoomAvailability(int roomId, bool expectedOutcome)
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = startDate.AddDays(1);
            mockRoomService.Setup(service => service.FindAvailableRoom(startDate, endDate)).Returns(roomId);
            var booking = new Booking { StartDate = startDate, EndDate = endDate, CustomerId = 1 };

            // Act
            var result = bookingManager.CreateBooking(booking);

            // Assert
            Assert.Equal(expectedOutcome, result);
            if (expectedOutcome)
            {
                mockBookingRepository.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Once); // Verifies that a booking was attempted to be added
            }
            else
            {
                mockBookingRepository.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Never); // Verifies that a booking was not added
            }
        }
        [Fact]
        public void CreateBooking_InvalidDates_ThrowsArgumentException()
        {
            // Arrange
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today, // End date before start date
                CustomerId = 1
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => bookingManager.CreateBooking(booking));
        }

        [Fact]
        public void CreateBooking_NullBooking_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => bookingManager.CreateBooking(null));
        }
    }
}