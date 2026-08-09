using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            this.attendanceService = attendanceService;
        }

        [HttpPost("RecordAttendance")]
        public async Task<IActionResult> RecordAttendance([FromBody] List<AttendanceD> record)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await attendanceService.AddAttendance(record);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("editAttend/{id}")]
        public async Task<IActionResult> UpdateAttend([FromRoute] string id, [FromBody] AttendanceD attendanceD)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await attendanceService.UpdateAttendance(attendanceD, id);
                return Ok("Succes Update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetAttendanceByDate")]
        public IActionResult GetAttendanceByDate([FromQuery] DateOnly date)
        {
            try
            {
                var attendances = attendanceService.GetAttendanceByDate(date);
                return Ok(attendances);
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("getBarberAttend/{id}")]
        public async Task<IActionResult> GetBarberAttend([FromRoute] string id, [FromQuery] Pagenation pagenation)
        {

            try
            {
                var attend = await attendanceService.GetAttendanceByBarberId(pagenation, id);
                return Ok(attend);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }


        [HttpGet("getDurationAttend")]
        public async Task<IActionResult> GetDurationAttend([FromQuery] Pagenation pagenation, [FromQuery] DurationBarberAttend durationBarberAttend)
        {
            try
            {
                var respons = await attendanceService.GetDurrationAttendance(pagenation, durationBarberAttend);

                return Ok(respons);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }
    }
}
