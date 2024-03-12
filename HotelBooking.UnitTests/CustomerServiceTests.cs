using Xunit;
using Moq;
using HotelBooking.Core;
using HotelBooking.Core.Services;

namespace HotelBooking.UnitTests
{
    public class CustomerServiceTests
    {
        private readonly CustomerService customerService;
        private readonly Mock<IRepository<Customer>> mockCustomerRepository;

        public CustomerServiceTests()
        {
            mockCustomerRepository = new Mock<IRepository<Customer>>();
            customerService = new CustomerService(mockCustomerRepository.Object);
        }

        [Theory]
        [InlineData(1, true)]  // Existing customer
        [InlineData(0, false)] // Typically, 0 is not a valid ID for existing customers
        [InlineData(-1, false)] // Negative values are not valid IDs
        [InlineData(1337, false)] // Assume 1337 does not correspond to an existing customer
        public void GetCustomerById_ReturnsCorrectCustomerInformation(int customerId, bool doesExist)
        {
            // Arrange
            var expectedCustomer = doesExist ? new Customer { Id = customerId, Name = "Test Name" } : null;
            mockCustomerRepository.Setup(repo => repo.Get(customerId)).Returns(expectedCustomer);

            // Act
            var result = customerService.GetCustomerById(customerId);

            // Assert
            if (doesExist)
            {
                Assert.NotNull(result);
                Assert.Equal(customerId, result.Id);
                Assert.Equal("Test Name", result.Name); // Ensure the retrieved customer matches expected values
            }
            else
            {
                Assert.Null(result); 
            }
        }
    }
}
