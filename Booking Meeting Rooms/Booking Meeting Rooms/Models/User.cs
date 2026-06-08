using Booking_Meeting_Rooms.Enums;
using Microsoft.AspNetCore.Identity;

namespace Booking_Meeting_Rooms.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }   

        public string? Password { get; set; }

        public List<Bookings> Bookings { get; set; } = new();

        public Role Role { get; set; }
    }
}
