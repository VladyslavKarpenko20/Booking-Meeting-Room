using Booking_Meeting_Rooms.Models;
using System.Threading.Tasks;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IEquipmentServices
    {
        Task<List<Equipment>> GetAllEquipment(int Page, int PageSize);

        Task AddEquipment(string Name);
        Task DeleteEquipment(int equipmentId);

    }
}
