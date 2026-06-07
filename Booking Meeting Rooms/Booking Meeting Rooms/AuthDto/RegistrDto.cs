using System.ComponentModel.DataAnnotations;

namespace Booking_Meeting_Rooms.AuthDto
{
    public class RegistrDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
