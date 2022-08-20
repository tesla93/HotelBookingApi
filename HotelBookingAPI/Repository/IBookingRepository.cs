using HotelBookingAPI.Data;
using HotelBookingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repository
{
    public interface IBookingRepository
    {
        Task<BookingDTO> CreateBooking(BookingDTO booking);
        Task<BookingDTO> UpdateBooking(int bookingId, Booking booking);
        Task<Booking> Get(Expression<Func<Booking, bool>> expression, List<string> includes = null);
        Task<List<DateTime>> GetAvailableDates();
        Task DeleteBooking(Booking booking);
        Task<bool> IsBooked(DateTime checkInDate, DateTime checkOutDate);
        Task<bool> IsBooked(DateTime checkInDate, DateTime checkOutDate, int id);
    }
}
