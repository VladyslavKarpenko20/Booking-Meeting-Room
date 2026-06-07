using Booking_Meeting_Rooms.Context;
using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_Meeting_Rooms.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AddDbContext _context;

        public RoomRepository(AddDbContext context)
        {
            _context = context;
        }

       public async Task<List<Room>> GetAllRooms(int Page , int PageSize)
       {
            return await _context.Rooms.Include(e => e.Equipment).Include(x => x.Bookings).Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();    
       }

        public async Task<Room?> GetRoomById(int? roomId)
        {
            return await _context.Rooms.Include(r => r.Equipment).Include(b => b.Bookings).FirstOrDefaultAsync(x => x.Id == roomId);
        }
        
        public async Task AddRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoom(Room room)
        {
            _context.Rooms.Remove(room);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoom(Room room)
        {
            _context.Rooms.Update(room);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Room>> GetRoomsWithBookingsByDate(DateTimeOffset StartTime , DateTimeOffset EndTime)
        {
            return await _context.Rooms.Include(x => x.Bookings.Where(b => b.StartTime >= StartTime && b.EndTime <= EndTime)).ToListAsync();
        }

    }
}
