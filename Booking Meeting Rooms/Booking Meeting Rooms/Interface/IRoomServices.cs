using Booking_Meeting_Rooms.Models;
using Booking_Meeting_Rooms.RoomsDto;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IRoomServices
    {
        Task<List<RoomReadDto>> GetAllRoom(int Page, int PageSize);

        Task<RoomReadDto> GetRoomById(int roomId);

        Task DeleteRoom(int roomId);

        Task UpdateRoom(int roomId, RoomWriteDto roomWrite);

        Task AddRoom(RoomWriteDto roomWrite);
    }
}
