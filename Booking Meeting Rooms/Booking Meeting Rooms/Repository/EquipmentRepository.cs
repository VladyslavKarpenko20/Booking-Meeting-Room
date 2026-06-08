using Booking_Meeting_Rooms.Context;
using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_Meeting_Rooms.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly AddDbContext _context;

        public EquipmentRepository(AddDbContext context)
        {
            _context = context;
        }


        public async Task<List<Equipment>> GetAllEquipment(int Page , int PageSize)
        {
            var equipment = await _context.Equipment.Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();

            return equipment;
        }

        public async Task AddEquipment(Equipment equipment)
        {
            await _context.Equipment.AddAsync(equipment);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEquipment(Equipment equipment)
        {
            _context.Equipment.Remove(equipment);

            await _context.SaveChangesAsync();
        }

        public async Task<Equipment?> FindEquipmentById(int equipmentId)
        {
            return await _context.Equipment.FirstOrDefaultAsync(e => e.Id == equipmentId);
        }

        public async Task<Equipment?> FindEquipment(string Name)
        {
            return await _context.Equipment.FirstOrDefaultAsync(e => e.Name != null && e.Name.ToLower() == Name.ToLower());
        }


        public async Task<List<Equipment>> GetEquipmentById(List<int> ints)
        {
            return await _context.Equipment.Where(x => ints.Contains(x.Id)).ToListAsync();
        }
    }
}
