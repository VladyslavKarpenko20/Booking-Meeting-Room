namespace Booking_Meeting_Rooms.Models
{
    public class Bookings
    {
        public int Id { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        
    }
}
