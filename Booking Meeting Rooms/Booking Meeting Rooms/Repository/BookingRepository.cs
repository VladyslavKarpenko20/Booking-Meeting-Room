using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Context;
using Booking_Meeting_Rooms.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_Meeting_Rooms.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AddDbContext _context;

        public BookingRepository(AddDbContext context)
        {
            _context = context;
        }

        public async Task AddBooking(Bookings bookings)
        {
            await _context.Bookings.AddAsync(bookings);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookings(Bookings bookings)
        {
            _context.Bookings.Remove(bookings);

            await _context.SaveChangesAsync();
        }

        public IQueryable<Bookings> GetMyBookings(int userId)
        {
            return _context.Bookings.Where(b => b.UserId == userId).Include(x => x.User).Include(x => x.Room).AsQueryable();
        }

        public async Task<Bookings?> GetBookingsById(int bookingId)
        {
            return await _context.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId);
        }

        public async Task<List<Bookings>> GetAllBookingsByRoomId(int roomId)
        {

            return await _context.Bookings.Where(b => b.RoomId == roomId).ToListAsync();
        }

        public async Task UpdateBooking(Bookings bookings)
        {
            _context.Bookings.Update(bookings);

            await _context.SaveChangesAsync();
        }
        
        public IQueryable<Bookings> GetQueryableBookingsByRoomId(int? roomId)
        {
            return _context.Bookings.Where(x => x.RoomId == roomId).AsQueryable();
        }

    }
}
