using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BookingRepository(HotelBookingContext context) : IRepository<Booking>
    {
        public void Add(Booking entity)
        {
            context.Booking.Add(entity);
            context.SaveChanges();
        }

        public void Edit(Booking entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Booking Get(int id)
        {
            return context.Booking.Include(b => b.Customer).Include(b => b.Room).FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Booking> GetAll()
        {
            return context.Booking.Include(b => b.Customer).Include(b => b.Room).ToList();
        }

        public void Remove(int id)
        {
            var booking = context.Booking.FirstOrDefault(b => b.Id == id);
            context.Booking.Remove(booking);
            context.SaveChanges();
        }

    }
}
