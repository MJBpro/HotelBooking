using System;

namespace HotelBooking.Core.Interfaces
{
    public interface IRoomService
    {
        int FindAvailableRoom(DateTime startDate, DateTime endDate);
        int GetTotalNumberOfRooms();
    }
}