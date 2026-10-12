using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
        [EnableRateLimiting("Login")]
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

        [Microsoft.AspNetCore.Authorization.Authorize]
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            if(!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(new { message = errors });
            }

            try
            {
                await _appUserService.ChangePassword(changePassword);
                return Ok(new { message = "تم تغيير كلمة المرور بنجاح" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}
