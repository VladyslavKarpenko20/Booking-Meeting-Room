using Booking_Meeting_Rooms.Models;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IEquipmentRepository
    {

        Task<List<Equipment>> GetAllEquipment(int Page, int PageSize);

        Task AddEquipment(Equipment equipment);

        Task<Equipment?> FindEquipment(string Name);

        Task DeleteEquipment(Equipment equipment);

        Task<Equipment?> FindEquipmentById(int equipmentId);

        Task<List<Equipment>> GetEquipmentById(List<int> ints);
    }
}
