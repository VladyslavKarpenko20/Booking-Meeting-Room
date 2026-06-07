using Booking_Meeting_Rooms.Models;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IBookingRepository
    {
        Task AddBooking(Bookings bookings);

        Task DeleteBookings(Bookings bookings);

        IQueryable<Bookings> GetMyBookings(int userId);

        Task<Bookings?> GetBookingsById(int bookingId);

        Task<List<Bookings>> GetAllBookingsByRoomId(int roomId);

        Task UpdateBooking(Bookings bookings);

        IQueryable<Bookings> GetQueryableBookingsByRoomId(int? roomId);

    }
}
