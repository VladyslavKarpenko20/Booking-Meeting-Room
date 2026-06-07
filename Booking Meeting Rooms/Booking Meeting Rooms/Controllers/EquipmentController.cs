using Booking_Meeting_Rooms.Enums;
using Booking_Meeting_Rooms.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Meeting_Rooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentServices _equipmentServices;

        public EquipmentController(IEquipmentServices equipmentServices)
        {
            _equipmentServices = equipmentServices;
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet("Get/All/Equipment/{Page:int}/{PageSize:int}")]
        public async Task<IActionResult> GetAllEquipment(int Page = 1, int PageSize = 10)
        {
            var res = await _equipmentServices.GetAllEquipment(Page, PageSize);

            return Ok(res);
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpPost("Add/Equipment/{Name}")]
        public async Task<IActionResult> AddEquipment(string Name)
        {
            await _equipmentServices.AddEquipment(Name);

            return Ok();
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpDelete("Delete/Equipment/{equipmentId:int}")]
        public async Task<IActionResult> DeleteEquipment(int equipmentId)
        {
            await _equipmentServices.DeleteEquipment(equipmentId);

            return Ok();
        }

    }
}
