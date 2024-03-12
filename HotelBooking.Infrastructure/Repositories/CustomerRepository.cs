using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;

namespace HotelBooking.Infrastructure.Repositories
{
    public class CustomerRepository(HotelBookingContext context) : IRepository<Customer>
    {
        public void Add(Customer entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Customer entity)
        {
            throw new NotImplementedException();
        }

        public Customer Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            return context.Customer.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
