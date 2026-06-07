using System.ComponentModel.DataAnnotations;

namespace Booking_Meeting_Rooms.AuthDto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }
    }
}
