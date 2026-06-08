using Booking_Meeting_Rooms.BookingDto;
using Booking_Meeting_Rooms.Enums;
using Booking_Meeting_Rooms.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Booking_Meeting_Rooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingServices _bookingServices;

        public BookingController(IBookingServices bookingServices)
        {
            _bookingServices = bookingServices;
        }


        [Authorize]
        [HttpGet("Get/My/Booking/{Page:int}/{PageSize:int}")]
        public IActionResult GetMyBooking(int Page = 1, int PageSize = 10, string Status = "all", DateTimeOffset? StartTime = null, DateTimeOffset? EndTime = null)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int result))
            {

                var res = _bookingServices.GetMyBookings(Page, PageSize, result, Status, StartTime, EndTime);

                return Ok(res);
            }
            else
                return Unauthorized("Failed to identify user from token");
        }

        [Authorize]
        [HttpPost("Add/Booking/{roomId:int}")]
        public async Task<IActionResult> AddBooking([FromBody] BookingWriteDto bookingWrite, int roomId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int result))
            {

                await _bookingServices.AddBooking(bookingWrite, result, roomId);

                return Ok();
            }
            else
                return Unauthorized("Failed to identify user from token");
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpDelete("Delete/Booking/{bookingId:int}")]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            await _bookingServices.DeleteBooking(bookingId);

            return Ok();
        }

        [Authorize]
        [HttpDelete("Delete/My/Booking/{bookingId:int}")]
        public async Task<IActionResult> DeleteMyBooking(int bookingId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int result))
            {

                await _bookingServices.DeleteMyBooking(bookingId, result);

                return Ok();
            }
            else
                return Unauthorized("Failed to identify user from token");


        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPut("Update/Booking/{bookingId:int}/{roomId:int}/{Email}")]
        public async Task<IActionResult> UpdateBooking([FromBody] BookingWriteDto bookingWrite ,int bookingId, int roomId, string Email)
        {
            await _bookingServices.UpdateBooking(bookingWrite, bookingId, roomId, Email);

            return Ok();
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet("Get/All/Booking")]
        public async Task<IActionResult> GetAllBooking(int Page = 1, int PageSize = 10, string Status = "all", int? roomId = null, DateTimeOffset? StartTime = null, DateTimeOffset? EndTime = null)
        {
            var list = await _bookingServices.GetAllBookings(Page,PageSize,Status,roomId,StartTime, EndTime);

            return Ok(list);
        }

    }
}
