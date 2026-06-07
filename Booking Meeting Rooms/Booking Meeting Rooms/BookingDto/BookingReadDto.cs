using Booking_Meeting_Rooms.Models;
using Booking_Meeting_Rooms.RoomsDto;
using Booking_Meeting_Rooms.UserDto;

namespace Booking_Meeting_Rooms.BookingDto
{
    public class BookingReadDto
    {
        public int Id { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public int RoomId { get; set; }

        public UserShortDto User { get; set; }

        public RoomShortDto Room { get; set; }

        public int UserId { get; set; }
    }
}
