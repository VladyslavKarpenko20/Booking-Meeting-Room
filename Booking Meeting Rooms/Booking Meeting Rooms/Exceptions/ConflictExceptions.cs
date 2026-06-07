namespace Booking_Meeting_Rooms.Exceptions
{
    public class ConflictExceptions : Exception
    {
        public ConflictExceptions(string massage) : base(massage) { }
    }
}
