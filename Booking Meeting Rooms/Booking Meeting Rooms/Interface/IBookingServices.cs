using Booking_Meeting_Rooms.BookingDto;
using Booking_Meeting_Rooms.Models;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IBookingServices
    {
        List<BookingReadDto> GetMyBookings(int Page, int PageSize, int userId ,string Status, DateTimeOffset? StartTime, DateTimeOffset? EndTime);

        Task AddBooking(BookingWriteDto bookingWrite, int userId, int roomId);

        Task DeleteBooking(int bookingId);

        Task DeleteMyBooking(int bookingId, int userId);

        Task UpdateBooking(BookingWriteDto bookingWrite, int bookingId, int roomId, string Email);

        Task<List<BookingReadDto>> GetAllBookings(int Page, int PageSize, string Status, int? roomId, DateTimeOffset? StartTime, DateTimeOffset? EndTime);
    }
}
