using HotelBooking.Core;
using HotelBooking.Core.Interfaces;

namespace HotelBooking.Mvc.Models
{
    public class BookingViewModel(IRepository<Booking> repository, IBookingManager manager) : IBookingViewModel
    {
        private int yearToDisplay;

        public IEnumerable<Booking> Bookings => repository.GetAll();

        public int YearToDisplay
        {
            get
            {
                var minBookingYear = MinBookingDate.Year;
                var maxBookingYear = MaxBookingDate.Year;
                return yearToDisplay < minBookingYear ? minBookingYear :
                    yearToDisplay > maxBookingYear ? maxBookingYear : yearToDisplay;
            }
            set => yearToDisplay = value;
        }

        public string GetMonthName(int month)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);
        }

        public bool DateIsOccupied(int year, int month, int day)
        {
            bool occupied = false;
            DateTime dt = new DateTime(year, month, day);
            if (day > DateTime.DaysInMonth(year, month)) return false;
            var occupiedDate = FullyOccupiedDates.FirstOrDefault(d => d == dt);
            if (occupiedDate > DateTime.MinValue)
                occupied = true;
            return occupied;
        }

        public List<DateTime> FullyOccupiedDates => manager.GetFullyOccupiedDates(MinBookingDate, MaxBookingDate);

        private DateTime MinBookingDate
        {
            get
            {
                var bookingStartDates = Bookings.Select(b => b.StartDate);
                var startDates = bookingStartDates as DateTime[] ?? bookingStartDates.ToArray();
                return startDates.Any() ? startDates.Min() : DateTime.MinValue;
            }
        }

        private DateTime MaxBookingDate
        {
            get
            {
                var bookingEndDates = Bookings.Select(b => b.EndDate);
                var dateTimes = bookingEndDates as DateTime[] ?? bookingEndDates.ToArray();
                return dateTimes.Any() ? dateTimes.Max() : DateTime.MaxValue;
            }
        }

    }
}
