using Booking_Meeting_Rooms.Enums;
using Booking_Meeting_Rooms.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Meeting_Rooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneratePdfController : ControllerBase
    {
        private readonly IPdfServices _pdfServices;

        public GeneratePdfController(IPdfServices pdfServices) 
        {
             _pdfServices = pdfServices;
        }


        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet("Generate/Pdf")] 
        public async Task<IActionResult> GetDailyReport([FromQuery] int year, [FromQuery] int mounth, [FromQuery] int day)
        {
            var date = await _pdfServices.GenerateDailyReportPdf(year, mounth, day);

            string FileName = $"Report_{year}_{mounth}_{day}.pdf";

            return File(date, "application/pdf", FileName);
        }

    }
}
