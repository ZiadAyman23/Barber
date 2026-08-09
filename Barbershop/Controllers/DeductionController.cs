using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionController : ControllerBase
    {
        private readonly IDeductionService deductionServes;

        public DeductionController(IDeductionService deductionServes)
        {
            this.deductionServes = deductionServes;
        }


        [HttpPost("addDeduction")]
        public async Task<IActionResult> AddDeduction([FromBody] DeductionD deductionD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await deductionServes.AddDeduction(deductionD);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("editDeduction/{id}")]
        public async Task<IActionResult> EditDeduction([FromRoute] string id, [FromBody] DeductionD deductionD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await deductionServes.EditDeduction(id, deductionD);
                return Ok();

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("removeDeduction/{id}")]
        public async Task<IActionResult> DeleteDeduction([FromRoute] string id)
        {
            try
            {
                await deductionServes.RemoveDeduction(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllDeduction")]
        public async Task<IActionResult> GetAllDeduction([FromQuery] Pagenation pagenation)
        {
            try
            {
                var deductions = await deductionServes.GetAllDeduction(pagenation);
                return Ok(deductions);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("getBarberDeduction/{id}")]
        public async Task<IActionResult> GetBarberDeduction([FromRoute] string id, [FromQuery] Pagenation pagenation)
        {
            try
            {
                var deductions = await deductionServes.GetAllBarberDeductions(id, pagenation);
                return Ok(deductions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getDeductionByDate")]
        public async Task<IActionResult> GetDeductionByDate([FromQuery] DateTime dateOnly)
        {


            try
            {
                var deduction = await deductionServes.GetAllDeductionByDated(dateOnly);
                return Ok(deduction);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }

        }

    }
}
