using System.Numerics;

namespace Booking_Meeting_Rooms.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Capacity { get; set; }
        public List<Equipment> Equipment { get; set; } = new();

        public List<Bookings> Bookings { get; set; } = new();   

    }
}
