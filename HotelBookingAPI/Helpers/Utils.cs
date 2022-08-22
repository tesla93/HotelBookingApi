using System;

namespace HotelBookingAPI.Helpers
{
    public static class Utils
    {
        public static bool DateBetweenIsGreaterThanThreeDays(DateTime checkInDate, DateTime checkOutDate)
        {
            return (checkOutDate - checkInDate).TotalDays > 3;
        }

        public static bool CheckOutIsGreaterThanThirtyDays(DateTime checkOutDate)
        {
            return (checkOutDate - DateTime.Now).TotalDays > 30;
        }

        public static bool CheckInIsGreaterThanOrEqualThanCheckOut(DateTime checkInDate, DateTime checkOutDate)
        {
            return (checkInDate - checkOutDate).TotalDays >= 0;
        }

        public static bool CheckInIsBeforeToday(DateTime checkInDate)
        {
            return (checkInDate - DateTime.Now).TotalDays < 0;
        }
    }
}