using Microsoft.AspNetCore.Mvc;

namespace Barbershop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        public AccountController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] Login model)
        {
            try
            {
                var tokens = await _appUserService.Login(model);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var tokens = await _appUserService.RefreshToken(request.RefreshToken);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Regisrer([FromBody]RegisterD registerD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _appUserService.Register(registerD);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("checkAccess")]
        public async Task<IActionResult> CheckAccess([FromBody] CheckAccess pass)
        {
            try
            {
                await _appUserService.CheckSecreatKey(pass);
                return Ok();
            }
            catch (Exception e)
            { return BadRequest(e.Message); }
        }

    }
}
