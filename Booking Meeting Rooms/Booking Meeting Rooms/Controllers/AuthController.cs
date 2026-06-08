using Microsoft.AspNetCore.Mvc;
using Booking_Meeting_Rooms.AuthDto;
using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Exceptions;

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

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Password) || string.IsNullOrWhiteSpace(loginDto.Email))
            {
                throw new BadRequestExceptions("Invalid Data");
            }
            else
            {
                var token = await _authServices.Login(loginDto.Email, loginDto.Password);

                return Ok(token);
            }
        }

    }
}
