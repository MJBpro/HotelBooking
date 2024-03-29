﻿using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;

namespace HotelBooking.Infrastructure.Repositories
{
    public class RoomRepository(HotelBookingContext context) : IRepository<Room>
    {
        public void Add(Room entity)
        {
            context.Room.Add(entity);
            context.SaveChanges();
        }

        public void Edit(Room entity)
        {
            throw new NotImplementedException();
        }

        public Room Get(int id)
        {
            // The FirstOrDefault method below returns null
            // if there is no room with the specified Id.
            return context.Room.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Room> GetAll()
        {
            return context.Room.ToList();
        }

        public void Remove(int id)
        {
            // The Single method below throws an InvalidOperationException
            // if there is not exactly one room with the specified Id.
            var room = context.Room.Single(r => r.Id == id);
            context.Room.Remove(room);
            context.SaveChanges();
        }
    }
}
