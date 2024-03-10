using HotelBooking.Core.Interfaces;

namespace HotelBooking.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> customerRepository;

        public CustomerService(IRepository<Customer> customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public Customer GetCustomerById(int customerId)
        {
            return customerRepository.Get(customerId);
        }

    }
}