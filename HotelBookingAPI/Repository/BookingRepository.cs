using AutoMapper;
using HotelBookingAPI.Context;
using HotelBookingAPI.Data;
using HotelBookingAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelBookingAPI.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly DatabaseContext _context;
        private readonly DbSet<Booking> _dbBooking;
        private readonly IMapper _mapper;
        public BookingRepository(DatabaseContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _dbBooking = _context.Set<Booking>();
        }

        public async Task<BookingDTO> CreateBooking(BookingDTO bookingDTO)
        {
            Booking booking = _mapper.Map<Booking>(bookingDTO);
            booking.Created ??= DateTime.Now;
            var addedBoooking = await _dbBooking.AddAsync(booking);
            await Save();
            return _mapper.Map<Booking, BookingDTO>(addedBoooking.Entity);

        }

        public async Task<BookingDTO> UpdateBooking(int bookingId, Booking booking)
        {
            booking.Updated = DateTime.Now;
            var updateBook= _dbBooking.Attach(booking);
            _context.Entry(booking).State = EntityState.Modified;
            await Save();
            return _mapper.Map<Booking, BookingDTO>(updateBook.Entity);
        }

        public Task DeleteBooking(int bookingId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> IsBooked(DateTime checkInDate, DateTime checkOutDate)
        {
            var existingBooking = await _dbBooking.FirstOrDefaultAsync(x => 
                       ((checkInDate < x.CheckOutDate && checkInDate.Date >= x.CheckInDate)
                       || (checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date <= x.CheckInDate.Date)
                       ));
            if (existingBooking != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsBooked(DateTime checkInDate, DateTime checkOutDate, int id)
        {
            var existingBooking = await _dbBooking.FirstOrDefaultAsync(x => (x.Id != id) &&
                       ((checkInDate< x.CheckOutDate && checkInDate.Date >= x.CheckInDate)
                       || (checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date <= x.CheckInDate.Date)
                       ));
            if (existingBooking != null)
            {
                return true;
            }
            return false;
        }
       

        public async Task<Booking> Get(Expression<Func<Booking, bool>> expression, List<string> includes = null)
        {
            IQueryable<Booking> query = _dbBooking;
            if (includes != null)
            {
                includes.ForEach(include =>
                   query = query.Include(include));
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
