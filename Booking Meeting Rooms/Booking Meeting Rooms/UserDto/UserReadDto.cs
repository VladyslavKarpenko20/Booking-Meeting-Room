using Booking_Meeting_Rooms.BookingDto;

namespace Booking_Meeting_Rooms.UserDto
{
    public class UserReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<BookingShortDto> Booking {  get; set; }
    }
}
