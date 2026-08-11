using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MonthelyReportController : ControllerBase
    {
        private readonly IMonthelyReportService monthelyReportService;

        public MonthelyReportController(IMonthelyReportService monthelyReportService)
        {
            this.monthelyReportService = monthelyReportService;
        }

        [HttpGet("monthelyReport")]
        public async Task<ActionResult> GetMonthReport([FromQuery]int Month, [FromQuery] int Year)
        {
            try
            {
                var respons = await monthelyReportService.GenrateMonthelyReport(Month, Year);
                return Ok(respons);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
