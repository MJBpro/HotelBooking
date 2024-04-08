using HotelBooking.Core;
using HotelBooking.Core.Interfaces;
using HotelBooking.Core.Services;
using Moq;
using Xunit;

namespace HotelBooking.Specs.Steps;

[Binding]
public class CreateBookingStepDefinitions
{
    private readonly BookingManager bookingManager;
    private readonly Mock<IRepository<Booking>> mockBookingRepository;
    private readonly Mock<IRoomService> mockRoomService;
    private readonly ScenarioContext scenarioContext;

    public CreateBookingStepDefinitions(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
        mockBookingRepository = new Mock<IRepository<Booking>>();
        mockRoomService = new Mock<IRoomService>();
        bookingManager = new BookingManager(mockBookingRepository.Object, mockRoomService.Object);
    }

    [Given(@"I have selected a room")]
    public void GivenIHaveSelectedARoom()
    {
        const int roomId = 1; 
        mockRoomService.Setup(rs => rs.FindAvailableRoom(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(roomId);
        scenarioContext["SelectedRoomId"] = roomId; 
    }

    [Given(@"no bookings exist for the dates (.*) to (.*)")]
    public void GivenNoBookingsExistForTheDates(string startDate, string endDate)
    {
        mockBookingRepository.Setup(repo => repo.GetAll()).Returns(new List<Booking>().AsQueryable());
    }

    [Given(@"a booking exists for the dates (.*) to (.*)")]
    public void GivenABookingExistsForTheDates(string startDate, string endDate)
    {
        var existingBooking = new Booking
        {
            StartDate = DateTime.Parse(startDate),
            EndDate = DateTime.Parse(endDate),
            IsActive = true,
            RoomId = 1 
        };
        mockBookingRepository.Setup(repo => repo.GetAll()).Returns(new List<Booking> { existingBooking }.AsQueryable());
    }

    [When(@"I create a booking for these dates (.*) to (.*)")]
    public void WhenICreateABookingForTheseDatesTo(string startDate, string endDate)
    {
        var booking = new Booking
        {
            StartDate = DateTime.Parse(startDate),
            EndDate = DateTime.Parse(endDate),
            CustomerId = 1, 
            RoomId = (int)scenarioContext["SelectedRoomId"],
            IsActive = true
        };

        try
        {
            var result = bookingManager.CreateBooking(booking);
            scenarioContext["BookingResult"] = result;
        }
        catch (Exception e)
        {
            scenarioContext["CaughtException"] = e;
        }
    }

    [When(@"I attempt to create a booking for these dates (.*) to (.*)")]
    public void WhenIAttemptToCreateABookingForTheseDatesTo(string startDate, string endDate)
    {
        WhenICreateABookingForTheseDatesTo(startDate, endDate); 
    }

    [Then(@"the booking should be (successful)")]
    public void ThenTheBookingShouldBeSuccessful(string outcome)
    {
        var expectedSuccess = outcome.Equals("successful", StringComparison.OrdinalIgnoreCase);
        var actualSuccess = (bool)scenarioContext["BookingResult"];
        Assert.Equal(expectedSuccess, actualSuccess);

        if (scenarioContext.TryGetValue("CaughtException", out var value))
        {
            Assert.Null(value); 
        }
    }
    [Then(@"the booking should be (declined)")]
    public void ThenTheBookingShouldBeDeclined(string outcome)
    {
        var expectedSuccess = outcome.Equals("declined", StringComparison.OrdinalIgnoreCase);
        var actualSuccess = (bool)scenarioContext["BookingResult"];
        Assert.Equal(expectedSuccess, actualSuccess);

        if (scenarioContext.TryGetValue("CaughtException", out var value))
        {
            Assert.Null(value); 
        }
    }
    
    [Then(@"an error should be thrown for invalid booking data")]
    public void ThenAnErrorShouldBeThrownForInvalidBookingData()
    {
        Assert.True(scenarioContext.ContainsKey("CaughtException"));
        var exception = scenarioContext["CaughtException"];
        Console.WriteLine(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }
    
    

    [Given(@"I have selected an invalid booking")]
    public void GivenIHaveSelectedAnInvalidBooking()
    {
        scenarioContext["booking"] = null;
    }

    [When(@"I attempt to book a any room")]
    public void WhenIAttemptToBookAAnyRoom()
    {
        scenarioContext.TryGetValue("booking", out Booking? booking);
        try
        {
            var result = bookingManager.CreateBooking(booking);
            scenarioContext["BookingResult"] = result;
        }
        catch (Exception e)
        {
            scenarioContext["CaughtException"] = e;
        }
    }

    [Then(@"an error should be thrown for invalid dates")]
    public void ThenAnErrorShouldBeThrownForInvalidDates()
    {
        Assert.True(scenarioContext.ContainsKey("CaughtException"));
        var exception = scenarioContext["CaughtException"];
        Console.WriteLine(exception);
        Assert.IsType<ArgumentException>(exception);
    }
}
