using HotelBookingAPI.Data;
using HotelBookingAPI.Models;
using System;
using System.Collections.Generic;

namespace HotelBookingUnitTest.Fixture
{
    public static class BookingModelFixture
    {
        public static List<BookingDTO> GetBookings() =>
            new()
            {
                new BookingDTO
                {
                    Id= 1,
                   CheckInDate= DateTime.Now.AddDays(1),
                   CheckOutDate= DateTime.Now.AddDays(2),
                   RoomNumber= 23,
                   Created= DateTime.Now,
                   Updated= DateTime.Now,
                   
                },
                new BookingDTO
                {
                    Id = 2,
                    CheckInDate = DateTime.Now.AddDays(3),
                    CheckOutDate = DateTime.Now.AddDays(5),
                    RoomNumber = 23,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,

                },
                new BookingDTO
                {
                    Id = 3,
                    CheckInDate = DateTime.Now.AddDays(1),
                    CheckOutDate = DateTime.Now.AddDays(2),
                    RoomNumber = 23,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                },
        

            };
    }
}
