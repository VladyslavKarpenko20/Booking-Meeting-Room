using Booking_Meeting_Rooms.AuthDto;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IAuthServices
    {
        Task Registr(RegistrDto registrDto);
        Task<string> Login(string Email, string Password);
    }
}
