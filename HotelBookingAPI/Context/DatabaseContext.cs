using HotelBookingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Context
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfiguration(new BookingConfiguration());
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
