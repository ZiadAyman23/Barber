using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            this.salaryService = salaryService;
        }


        [HttpGet("GetCurrentSalaries")]
        public async Task<IActionResult> GetCurrentSalaries()
        {
            var salaries = await salaryService.GetCuurentSalaries();
            return Ok(salaries);
        }

        [HttpPost("AddSalary")]
        public async Task<IActionResult> AddSalary([FromBody] SalaryD salary)
        {
            try
            {
                await salaryService.AddSalary(salary);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllPaidSalary")]
        public async Task<IActionResult> GetAllpaidSalary([FromQuery]Pagenation pagenation)
        {
            try
            {
                var res = await salaryService.GetAllSalary(pagenation);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("getBarberSalary/{id}")]
        public async Task<IActionResult> GetBarberSalary([FromRoute]string id ,[FromQuery] Pagenation pagenation)
        {
            try
            {
                var res = await salaryService.GetSalaryByBarberId(id, pagenation);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("getSalaryDetails/{id}")]
        public async Task<IActionResult> GetSalaryDetails([FromRoute]string id)
        {
            try
            {
                var res = await salaryService.GetSalaryDetails(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
