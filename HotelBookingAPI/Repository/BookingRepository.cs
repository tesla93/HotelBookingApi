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
            var updateBook = _dbBooking.Attach(booking);
            _context.Entry(booking).State = EntityState.Modified;
            await Save();
            return _mapper.Map<Booking, BookingDTO>(updateBook.Entity);
        }

        public async Task DeleteBooking(Booking booking)
        {
            _dbBooking.Remove(booking);
            await Save();
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
                       ((checkInDate < x.CheckOutDate && checkInDate.Date >= x.CheckInDate)
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



        public async Task<List<DateTime>> GetAvailableDates()
        {
            var startDate = DateTime.Now.Date.AddDays(1);
            var endDate = DateTime.Now.Date.AddDays(30);

            var rangeDates = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
           .Select(offset => startDate.AddDays(offset))
           .ToList();

            var listOfBookings = await GetAll(orderBy: orderingFunc);

            List<DateTime> bookedDates = new List<DateTime>();

            listOfBookings.ForEach(booking =>
              {
                  var dates = Enumerable.Range(0, booking.CheckOutDate.Date.Subtract(booking.CheckInDate.Date).Days)
                  .Select(offset => booking.CheckInDate.Date.AddDays(offset))
                  .ToList();
                  bookedDates.AddRange(dates);
              });
            var availableDates = rangeDates.Except(bookedDates).ToList();

            return availableDates;
        }

        public async Task<List<Booking>> GetAll(Expression<Func<Booking, bool>> expression = null, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null, List<string> includes = null)
        {
            IQueryable<Booking> query = _dbBooking;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                includes.ForEach(include =>
                   query = query.Include(include)
               );
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.AsNoTracking().ToListAsync();
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

        Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderingFunc =
        query => query.OrderBy(booking => booking.CheckInDate);
    }
}
