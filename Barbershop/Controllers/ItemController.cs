global using Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("addItem")]
        public async Task<IActionResult> AddItem([FromBody] ItemD itemD)
        {
            try
            {
                await _itemService.AddItem(itemD);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateItem/{id}")]
        public async Task<IActionResult> UpdateItem([FromBody] ItemD itemD, [FromRoute] string id)
        {
            try
            {
                await _itemService.UpdateItem(itemD, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteItem/{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                await _itemService.RemoveItem(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getItemById/{id}")]
        public async Task<IActionResult> GetItemById(string id)
        {
            try
            {
                var item = await _itemService.GetItemById(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {
                var items = await _itemService.GetItems();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
