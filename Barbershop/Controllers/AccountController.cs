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
                Console.WriteLine($"'{model.Name}'");
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
                Console.WriteLine($"[AccountController] RefreshToken request received. Token: '{request?.RefreshToken}'");
                var tokens = await _appUserService.RefreshToken(request.RefreshToken);
                Console.WriteLine("[AccountController] RefreshToken call succeeded, returning tokens.");
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AccountController] RefreshToken error: {ex}");
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
