using System;
using System.Collections.Generic;

namespace HotelBooking.Core.Interfaces
{
    public interface IBookingManager
    {
        bool CreateBooking(Booking booking);
        
        List<DateTime> GetFullyOccupiedDates(DateTime startDate, DateTime endDate);
    }
}
