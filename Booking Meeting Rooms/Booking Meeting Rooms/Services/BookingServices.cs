using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Exceptions;
using Booking_Meeting_Rooms.Models;
using Booking_Meeting_Rooms.BookingDto;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Booking_Meeting_Rooms.UserDto;
using Booking_Meeting_Rooms.RoomsDto;

namespace Booking_Meeting_Rooms.Services
{
    public class BookingServices : IBookingServices
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;

        public BookingServices(IBookingRepository bookingRepository, IRoomRepository roomRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
        }

        public List<BookingReadDto> GetMyBookings(int Page, int PageSize, int userId, string Status, DateTimeOffset? StartTime, DateTimeOffset? EndTime)
        {
            if (Page < 1 || PageSize > 50 || PageSize < 1)
                throw new BadRequestExceptions("Invalid Data");

            if (StartTime > EndTime)
                throw new BadRequestExceptions("Invalid Data");

            var res = _bookingRepository.GetMyBookings(userId);

            var MyBooking = res.AsQueryable();

            var curentTime = DateTimeOffset.UtcNow;

            if (StartTime != null)
                MyBooking = MyBooking.Where(x => x.StartTime >= StartTime);

            if (EndTime != null)
                MyBooking = MyBooking.Where(x => x.EndTime <= EndTime);


            if (Status.ToLower() == "upcoming")
                MyBooking = MyBooking.Where(x => x.StartTime > curentTime);
            else if (Status.ToLower() == "past")
                MyBooking = MyBooking.Where(x => x.EndTime < curentTime);
            else if (Status.ToLower() == "active")
                MyBooking = MyBooking.Where(x => x.StartTime <= curentTime && x.EndTime >= curentTime);
            else if (Status.ToLower() != "all")
                throw new BadRequestExceptions("Invalid Data");


            var result = MyBooking.Select(b => new BookingReadDto
            {
                Id = b.Id,
                EndTime = b.EndTime,
                StartTime = b.StartTime,
                RoomId = b.RoomId,
                UserId = b.UserId,
                User = new UserShortDto
                {
                    Id = b.User.Id,
                    Name = b.User.Name,
                    Email = b.User.Email
                },
                Room = new RoomShortDto
                {
                    Id = b.Room.Id,
                    Capasity = b.Room.Capacity,
                    Name = b.Room.Name
                }


            }).Skip((Page - 1) * PageSize).Take(PageSize).ToList();

            return result;
        }

        public async Task DeleteBooking(int bookingId)
        {
            var res = await _bookingRepository.GetBookingsById(bookingId);

            if (res == null)
                throw new NotFoundExceptions("Booking Not Found");

            await _bookingRepository.DeleteBookings(res);
        }

        public async Task AddBooking(BookingWriteDto bookingWrite, int userId, int roomId )
        {
            DateTimeOffset time;

            try
            {
                 time = new DateTimeOffset(year: bookingWrite.Year, month: bookingWrite.Month, day: bookingWrite.Day, hour: bookingWrite.Hour, minute: 0, second: 0, TimeSpan.Zero);
            }
            catch (ArgumentOutOfRangeException)
            {

                throw new BadRequestExceptions("Invalid date or time format");
            }

            if (bookingWrite.Hour < 8 || bookingWrite.Hour > 17)
                throw new BadRequestExceptions("Invalid Data");

            if (await _roomRepository.GetRoomById(roomId) == null)
                throw new NotFoundExceptions("Room Not Found");

            if (DateTime.UtcNow > time)
                throw new BadRequestExceptions("Cannot book in the past");


            var res =  _bookingRepository.GetMyBookings(userId);

            if (res.Count() >= 2)
                throw new BadRequestExceptions("You cannot have more than 2 reservations");

            if (res.Any(x => x.StartTime == time))
                throw new ConflictExceptions("You already have a reservation for this time");

            var list = await _bookingRepository.GetAllBookingsByRoomId(roomId);


            var booking = new Bookings
            {
                RoomId = roomId,
                UserId = userId,
                StartTime = time,
                EndTime = time.AddHours(1),
            };


            if (list.Any(x => x.StartTime == time))
                throw new ConflictExceptions("This time is already booked");


            await _bookingRepository.AddBooking(booking);
        }

        public async Task DeleteMyBooking(int bookingId, int userId)
        {

            var bookings = await _bookingRepository.GetBookingsById(bookingId);

            if (bookings == null || bookings.UserId != userId)
                throw new NotFoundExceptions("Booking Not Found");

            if (bookings.EndTime > DateTimeOffset.UtcNow)
                throw new NotFoundExceptions("Bookings Not Found");

            await _bookingRepository.DeleteBookings(bookings);
        }

        public async Task UpdateBooking(BookingWriteDto bookingWrite ,int bookingId, int roomId, string Email)
        {
            DateTimeOffset time;

            var user = await _userRepository.GetUserByEmail(Email);

            if (user == null)
                throw new NotFoundExceptions("User Not Found");

            try
            {
                time = new DateTimeOffset(year: bookingWrite.Year, month: bookingWrite.Month, day: bookingWrite.Day, hour: bookingWrite.Hour, minute: 0, second: 0, TimeSpan.Zero);
            }
            catch (ArgumentOutOfRangeException)
            {

                throw new BadRequestExceptions("Invalid date or time format");
            }

            if (bookingWrite.Hour < 8 || bookingWrite.Hour > 17)
                throw new BadRequestExceptions("Invalid Data");

            if (await _roomRepository.GetRoomById(roomId) == null)
                throw new NotFoundExceptions("Room Not Found");

            if (DateTime.UtcNow > time)
                throw new BadRequestExceptions("Cannot book in the past");


            var bookingsByID = await _bookingRepository.GetBookingsById(bookingId);


            if (bookingsByID == null)
                throw new NotFoundExceptions("Booking Not Found");

            var MyBookings = _bookingRepository.GetMyBookings(user.Id);

            if (MyBookings.Any(x => x.StartTime == time && x.Id != bookingId))
                throw new ConflictExceptions("This user already has a reservation for this time");

            var bookingByRoom = await _bookingRepository.GetAllBookingsByRoomId(roomId);

            if (bookingByRoom.Any(x => x.StartTime == time && x.Id != bookingId))
                throw new ConflictExceptions("This time is already booked");


            bookingsByID.StartTime = time;
            bookingsByID.EndTime = time.AddHours(1);
            bookingsByID.RoomId = roomId;


            await _bookingRepository.UpdateBooking(bookingsByID);

        }

        public async Task<List<BookingReadDto>> GetAllBookings(int Page , int PageSize, string Status , int? roomId, DateTimeOffset? StartTime, DateTimeOffset? EndTime)
        {
            if (Page < 1 || PageSize > 50 || PageSize < 1)
                throw new BadRequestExceptions("Invalid Data");

            if(StartTime != null && EndTime != null && StartTime > EndTime)
                throw new BadRequestExceptions("Invalid Data");

            var room = await _roomRepository.GetRoomById(roomId);

            if (room == null)
                throw new NotFoundExceptions("Room Not Found");



            var list = _bookingRepository.GetQueryableBookingsByRoomId(roomId);

            var booking = list.AsQueryable();

            var curentTime = DateTime.UtcNow;


            if (StartTime != null) 
                booking = booking.Where(x => x.StartTime >= StartTime);

            if (EndTime != null)
                booking = booking.Where(x => x.EndTime <= EndTime);


            if (Status .ToLower() == "upcoming")
                booking = booking.Where(x => x.StartTime > curentTime);
            else if (Status.ToLower() == "past")
                booking = booking.Where(x => x.EndTime < curentTime);
            else if (Status.ToLower() == "active")
                booking = booking.Where(x => x.StartTime <= curentTime && x.EndTime >= curentTime);
            else if (Status.ToLower() != "all")
                throw new BadRequestExceptions("Invalid Data");


            var res = booking.Select(b => new BookingReadDto
            {
                Id = b.Id,
                EndTime = b.EndTime,
                StartTime = b.StartTime,
                RoomId = b.RoomId,
                UserId = b.UserId,
                User = new UserShortDto
                {
                    Id = b.User.Id,
                    Name = b.User.Name,
                    Email = b.User.Email
                },
                Room = new RoomShortDto
                {
                    Id = b.Room.Id,
                    Capasity = b.Room.Capacity,
                    Name = b.Room.Name
                }


            }).Skip((Page - 1) * PageSize).Take(PageSize).ToList();

            return res;
        }
    }
}
