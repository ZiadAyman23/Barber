using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }


        [HttpPost("creatOrder")]
        public async Task<IActionResult> CrateOrder([FromBody] OrderD order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await orderService.AddOrderAsync(order);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("editOrder/{id}")]
        public async Task<IActionResult> EditOrder([FromRoute] string id, [FromBody] OrderD order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await orderService.UpdateOrderAsync(order, id);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("getAllOrders")]
        public async Task<IActionResult> GatAllOrder([FromQuery] Pagenation pagenation)
        {
            try
            {
                var data = await orderService.GetAllOrdersAsync(pagenation);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("getBarberOrders/{id}")]
        public async Task<IActionResult> GetBarberOrders([FromRoute] string id, [FromQuery] Pagenation pagenation)
        {
            try
            {
                var data = await orderService.GetAllBarberOrdersAsync(pagenation, id);
                return Ok(data);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("getOrderByDate")]
        public async Task<IActionResult> GetOrderByDate([FromQuery] DateTime date)
        {
            try
            {
                var data = await orderService.GetOrderByDate(date);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("getOrderById/{id}")]
        public async Task<IActionResult> GetOrderByID([FromRoute] string id)
        {
            try
            {
                var data = await orderService.GetOrderAsync(id);
                Console.WriteLine(id);
                Console.WriteLine(data.BarberName);
                return Ok(data);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e);
            }
            catch (Exception ex)
            { return BadRequest(ex); }
        }

        [HttpDelete("removeOrder/{id}")]
        public async Task<IActionResult> RemoveOrder(string id)
        {
            try
            {
                await orderService.DeleteOrderAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch(Exception ex)
            {
                return BadRequest($"Unable to remove order {id}");
            }
        }
    }
}
