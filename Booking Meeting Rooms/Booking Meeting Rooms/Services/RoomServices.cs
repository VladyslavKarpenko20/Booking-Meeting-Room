using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.RoomsDto;
using Booking_Meeting_Rooms.Exceptions;
using Booking_Meeting_Rooms.Models;
using Booking_Meeting_Rooms.BookingDto;


namespace Booking_Meeting_Rooms.Services
{
    public class RoomServices  : IRoomServices
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        public RoomServices(IRoomRepository roomRepository, IEquipmentRepository equipmentRepository)
        {
            _roomRepository = roomRepository;
            _equipmentRepository = equipmentRepository;
        }


        public async Task<List<RoomReadDto>> GetAllRoom(int Page , int PageSize)
        {
            if (Page < 1 || PageSize < 1 || PageSize > 50)
                throw new BadRequestExceptions("Invalid Data");

            var res = await _roomRepository.GetAllRooms(Page, PageSize);

            var list = res.Select(x => new RoomReadDto
            {
                Id = x.Id,
                Name = x.Name,
                Capasity = x.Capacity,

                Bookings = x.Bookings.Select(b => new BookingShortDto
                {
                    Id = b.Id,
                    EndTime = b.EndTime,
                    StartTime = b.StartTime,
                    UserId  = b.UserId
                }).ToList(),
                Equipment = x.Equipment.Select(e => new Equipment 
                {
                    Id = e.Id,
                    Name = e.Name       
                }).ToList()

            }).ToList();

            return list;
        }

        public async Task<RoomReadDto> GetRoomById(int roomId)
        {
            var res = await _roomRepository.GetRoomById(roomId);

            if (res == null)
                throw new NotFoundExceptions("Room Not Found");


            var list = new RoomReadDto
            {
                Id = res.Id,
                Name = res.Name,
                Capasity = res.Capacity,

                Bookings = res.Bookings.Select(b => new BookingShortDto
                {
                    Id = b.Id,
                    EndTime = b.EndTime,
                    StartTime = b.StartTime,
                    UserId = b.UserId
                }).ToList(),
                Equipment = res.Equipment.Select(e => new Equipment
                { 
                    Id = e.Id,
                    Name = e.Name

                }).ToList()

            };

            return list;
        }

        public async Task DeleteRoom(int roomId)
        {
            var res = await _roomRepository.GetRoomById(roomId);

            if (res == null)
                throw new NotFoundExceptions("Room Not Found");

            await _roomRepository.DeleteRoom(res);
        }

        public async Task UpdateRoom(int roomId, RoomWriteDto roomWrite)
        {
            if (roomWrite.Capacity < 1 || string.IsNullOrWhiteSpace(roomWrite.Name))
                throw new BadRequestExceptions("Invalid Data");

            var res = await _roomRepository.GetRoomById(roomId);

            if (res == null)
                throw new NotFoundExceptions("Room Not Found");

            var equipments = await _equipmentRepository.GetEquipmentById(roomWrite.Equipment);

            res.Equipment = equipments;

            await _roomRepository.UpdateRoom(res);
        }

        public async Task AddRoom(RoomWriteDto roomWrite)
        {
            if (roomWrite.Capacity < 1 || string.IsNullOrWhiteSpace(roomWrite.Name))
                throw new BadRequestExceptions("Invalid Data");

            var equipment = await _equipmentRepository.GetEquipmentById(roomWrite.Equipment);

            var room = new Room
            {
                Capacity = roomWrite.Capacity,
                Name = roomWrite.Name,
                Equipment = equipment
            };

            await _roomRepository.AddRoom(room);

        }

    }
}
