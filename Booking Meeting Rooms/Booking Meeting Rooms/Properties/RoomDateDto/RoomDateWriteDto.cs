using Booking_Meeting_Rooms.Models;

namespace Booking_Meeting_Rooms.Properties.RoomDateDto
{
    public class RoomDateWriteDto
    {
        public int RoomId { get; set; }

        public int CountBooking { get; set; }

        public List<BookingDate> Bookings { get; set; } = new();


    }
}
