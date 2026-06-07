using Booking_Meeting_Rooms.Interface;

namespace Booking_Meeting_Rooms.Services
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;


        public UserServices(IUserRepository userRepository) 
        {
         _userRepository = userRepository;
        }


    }
}
