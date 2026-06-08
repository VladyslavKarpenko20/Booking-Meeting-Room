using Booking_Meeting_Rooms.Models;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IEquipmentServices
    {
        Task<List<Equipment>> GetAllEquipment(int Page, int PageSize);

        Task AddEquipment(string Name);
        Task DeleteEquipment(int equipmentId);

    }
}
