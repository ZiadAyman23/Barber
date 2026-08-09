using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpPost("add-expense")]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseD expense)
        {
            try
            {
                await _expenseService.AddExpense(expense);
                return Ok("Expense Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-expense/{id}")]
        public async Task<IActionResult> UpdateExpense([FromRoute] string id, [FromBody] ExpenseD expense)
        {
            try
            {
                await _expenseService.UpdateExpense(id, expense);
                return Ok("Expense updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-expenses")]
        public async Task<IActionResult> GetAllExpenses([FromQuery] Pagenation pagenation)
        {
            try
            {
                var expenses = await _expenseService.GetAllExpenses(pagenation);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-expense/{id}")]
        public async Task<IActionResult> GetExpense([FromRoute] string id)
        {
            try
            {
                var expense = await _expenseService.GetExpenses(id);
                return Ok(expense);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getByDate")]
        public async Task<IActionResult> GetExpensByDate([FromQuery] DateOnly date)
        {
            try
            {
                var data = await _expenseService.GetByDate(date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove-expense/{id}")]
        public async Task<IActionResult> RemoveExpense([FromRoute] string id)
        {
            try
            {
                await _expenseService.RemoveExpense(id);
                return Ok("Expense removed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
