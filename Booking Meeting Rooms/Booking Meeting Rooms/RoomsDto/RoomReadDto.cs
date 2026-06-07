using Booking_Meeting_Rooms.Models;
using Booking_Meeting_Rooms.BookingDto;

namespace Booking_Meeting_Rooms.RoomsDto
{
    public class RoomReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capasity { get; set; }

        public List<Equipment> Equipment { get; set; } = new();

        public List<BookingShortDto> Bookings { get; set; } = new();
    }
}
