using Booking_Meeting_Rooms.Models;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRooms(int Page, int PageSize);

        Task<Room?> GetRoomById(int? roomId);

        Task AddRoom(Room room);

        Task DeleteRoom(Room room);

        Task UpdateRoom(Room room);

        Task<List<Room>> GetRoomsWithBookingsByDate(DateTimeOffset StartTime, DateTimeOffset EndTime);

    }
}
