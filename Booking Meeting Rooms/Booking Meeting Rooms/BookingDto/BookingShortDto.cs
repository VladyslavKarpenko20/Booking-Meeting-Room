namespace Booking_Meeting_Rooms.BookingDto
{
    public class BookingShortDto
    {
        public int Id { get; set; }

        public int UserId {  get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        
    }
}
