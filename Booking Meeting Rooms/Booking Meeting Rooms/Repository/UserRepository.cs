using Booking_Meeting_Rooms.Context;
using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_Meeting_Rooms.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AddDbContext _context;

        public UserRepository(AddDbContext context) 
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmail(string Email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            return user;
        }

        public async Task AddUser(User user)
        {
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();
        }

    }
}
