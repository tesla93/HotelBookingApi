using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingAPI.Data
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public string GuestName{ get; set; }
        public int RoomNumber { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        //public int UserId { get; set; }

        //[ForeignKey(nameof(UserId))]
        //public virtual AppUser User { get; set; }

    }
}
