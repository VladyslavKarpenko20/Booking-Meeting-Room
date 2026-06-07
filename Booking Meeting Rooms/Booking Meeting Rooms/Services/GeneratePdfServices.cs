using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Properties.RoomDateDto;
using Booking_Meeting_Rooms.Exceptions;
using System.Runtime.CompilerServices;
using QuestPDF.Fluent;
using System.Reflection.Metadata;

namespace Booking_Meeting_Rooms.Services
{
    public class GeneratePdfServices : IPdfServices
    {
        private readonly IRoomRepository _roomRepository;


        public GeneratePdfServices(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }


        public async Task<byte[]> GenerateDailyReportPdf(int year, int month, int day)
        {
            DateTimeOffset startDay;

            try
            {
                startDay = new DateTimeOffset(year, month, day, 0, 0, 0, TimeSpan.Zero);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new BadRequestExceptions("Invalid Data");
            }

            DateTimeOffset endDate = startDay.AddDays(1);

            var list = await _roomRepository.GetRoomsWithBookingsByDate(startDay, endDate);
            var roomsFromDb = list.ToList();

   
            var reportDate = roomsFromDb.Select(x => new RoomDateWriteDto
            {
                CountBooking = x.Bookings.Count, 
                RoomId = x.Id,

                Bookings = x.Bookings.Select(b => new BookingDate
                {
                    StartHour = b.StartTime.Hour
                }).OrderBy(b => b.StartHour).ToList()

            }).ToList();

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(QuestPDF.Helpers.PageSizes.A4);

                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);

                    page.Header()
                    .Text($"Room occupancy report for {year} {month} {day}")
                    .Bold().FontSize(18).FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);


                    page.Content().PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre).Column(colum =>
                    {

                        foreach(var room in reportDate)
                        {
                            colum.Item().PaddingBottom(5).Text($"Room {room.RoomId} Booked on {room.CountBooking} hour ").Bold().FontSize(14);

                            foreach (var booking in room.Bookings)
                            {
                                colum.Item().Text($"Booked on {booking.StartHour}:00").FontSize(11).FontColor(QuestPDF.Helpers.Colors.Grey.Darken2);
                            }
                        }

                        colum.Item().PaddingBottom(15);

                    }); 
                });
            });

            return document.GeneratePdf();
        }
    }
}
