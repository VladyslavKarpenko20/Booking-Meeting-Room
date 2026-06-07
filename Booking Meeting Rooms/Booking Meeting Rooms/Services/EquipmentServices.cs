using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Models;
using Booking_Meeting_Rooms.Exceptions;

namespace Booking_Meeting_Rooms.Services
{
    public class EquipmentServices : IEquipmentServices
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentServices(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public async Task<List<Equipment>> GetAllEquipment(int Page, int PageSize)
        {
            if (Page < 1 || PageSize > 50 || PageSize < 1)
                throw new BadRequestExceptions("Invalid Data");

            var equipment = await _equipmentRepository.GetAllEquipment(Page, PageSize);

            return equipment;
        }

        public async Task AddEquipment(string Name)
        {
            var res = await _equipmentRepository.FindEquipment(Name);

            if (res != null)
                throw new ConflictExceptions("Such equipment already exists");

            var equipment = new Equipment
            {
                Name = Name
            };

            await _equipmentRepository.AddEquipment(equipment);
        }

        public async Task DeleteEquipment(int equipmentId)
        {
            var res = await _equipmentRepository.FindEquipmentById(equipmentId);
            
            if (res == null)
                throw new NotFoundExceptions("Equipment Not Found");

            await _equipmentRepository.DeleteEquipment(res);
        }
    }
}
