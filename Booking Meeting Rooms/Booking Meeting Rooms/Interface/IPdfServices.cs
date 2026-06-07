using Booking_Meeting_Rooms.Properties.RoomDateDto;

namespace Booking_Meeting_Rooms.Interface
{
    public interface IPdfServices
    {
        Task<byte[]> GenerateDailyReportPdf(int year, int month, int day);
    }
}
