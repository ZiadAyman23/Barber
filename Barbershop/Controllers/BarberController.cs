using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    
    public class BarberController : ControllerBase
    {
        private readonly IBarberService barberService;

        public BarberController(IBarberService barberService)
        {
            this.barberService = barberService;
        }



        [HttpPost("addBarber")]
        public async Task<IActionResult> AddBarber([FromBody] BarberD barber)
        {
            try
            {
               await barberService.AddBarber(barber);
                return Ok("Barber added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding barber: {ex.Message}");
            }
        }


        [HttpPut("updateBarber/{id}")]
        public async Task<IActionResult> UpdateBarber(string id, [FromBody] BarberD barber)
        {
            try
            {
              await  barberService.EditBarber(id, barber);
                return Ok("Barber updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating barber: {ex.Message}");
            }
        }

        [HttpDelete("deleteBarber/{id}")]
        public async Task<IActionResult> DeleteBarber(string id)
        {
            try
            {
              await  barberService.RemoveBarber(id);
                return Ok("Barber deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting barber: {ex.Message}");
            }
        }


        [HttpGet("getBarberById/{id}")]
        public async Task<IActionResult> GetBarber(string id)
        {
            try
            {
                var barber =await barberService.GetBarber(id);

                return Ok(barber);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving barber: {ex.Message}");
            }
        }


        [HttpGet("getAllBarbers")]
        public async Task<IActionResult> GetAllBarbers()
        {
            try
            {
                var barbers =await barberService.GetAllBarbers();
                return Ok(barbers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving barbers: {ex.Message}");
            }
        }
    }
}
