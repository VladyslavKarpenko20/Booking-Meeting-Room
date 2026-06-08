using Microsoft.AspNetCore.Mvc;
using Booking_Meeting_Rooms.AuthDto;
using Booking_Meeting_Rooms.Interface;

namespace Booking_Meeting_Rooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("Registr")]
        public async Task<IActionResult> Registr([FromBody] RegistrDto registrDto)
        {
            await _authServices.Registr(registrDto);

            return Ok();
        }

        [HttpPut("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var token = await _authServices.Login(Email, Password);

            return Ok(token);
        }

    }
}
