using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI.Models
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomNumber { get; set; }
        //public int UserId { get; set; }
    }
}
