namespace HotelBooking.Core.Interfaces
{
    public interface ICustomerService
    {
        Customer GetCustomerById(int customerId);
    }
}