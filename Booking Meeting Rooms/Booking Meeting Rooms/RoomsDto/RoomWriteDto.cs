
namespace Booking_Meeting_Rooms.RoomsDto
{
    public class RoomWriteDto
    {
        public string? Name { get; set; }

        public int Capacity { get; set; }

        public List<int> Equipment { get; set; } = new();
    }
}
