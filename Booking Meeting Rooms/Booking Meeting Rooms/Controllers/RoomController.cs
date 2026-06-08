using Booking_Meeting_Rooms.Enums;
using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.RoomsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Meeting_Rooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private readonly IRoomServices _roomServices;

        public RoomController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        [HttpGet("Get/Rooms/{Page:int}/{PageSize:int}")]
        public async Task<IActionResult> GetRooms(int Page = 1, int PageSize = 10)
        {
            var res = await _roomServices.GetAllRoom(Page,PageSize);

            return Ok(res);
        }

        [HttpGet("Get/Room/By/Id/{roomId:int}")]
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            var res = await _roomServices.GetRoomById(roomId);

            return Ok(res);
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("Add/Room")]
        public async Task<IActionResult> AddRoom([FromBody] RoomWriteDto roomWrite)
        {
            await _roomServices.AddRoom(roomWrite);

            return Ok();
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpDelete("Delete/Room/{roomId:int}")]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            await _roomServices.DeleteRoom(roomId);

            return Ok();
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPut("Update/Room/{roomId:int}")]
        public async Task<IActionResult> UpdateRoom(int roomId, RoomWriteDto roomWrite)
        {
            await _roomServices.UpdateRoom(roomId, roomWrite);


            return Ok();
        }


    }
}
