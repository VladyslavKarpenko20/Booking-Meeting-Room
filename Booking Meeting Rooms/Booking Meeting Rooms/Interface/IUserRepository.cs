using Booking_Meeting_Rooms.Models;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string Email);

        Task AddUser(User user);
    }
}
