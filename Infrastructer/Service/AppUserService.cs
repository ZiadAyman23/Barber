global using Core.DTO;
global using Core.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructer.Service
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration _configuration;
        public AppUserService(UserManager<AppUser> userManager,IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<Tokens> Login(Login login)
        {
            Console.WriteLine($"'{login.Name}'");
            var user = await _userManager.FindByNameAsync(login.Name);
            if (user == null)
            {
              
                throw new Exception("Invalid username");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid password or username");
            }

            Tokens tokens = await GenratTokens(user);
            if (tokens == null)
            {
                throw new Exception("Failed to generate tokens");
            }

            return tokens;

        }

        public async Task<Tokens> RefreshToken(string refreshToken)
        {
            Console.WriteLine($"[AppUserService] RefreshToken called with token: '{refreshToken}'");
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                Console.WriteLine($"[AppUserService] User not found for refresh token: '{refreshToken}'");
                throw new Exception("Invalid refresh token");
            }
            Console.WriteLine($"[AppUserService] User found: {user.UserName}. Expiration time: {user.RefreshTokenExpirationTime}, Current UTC: {DateTime.UtcNow}");
            if (user.RefreshTokenExpirationTime < DateTime.UtcNow)
            {
                Console.WriteLine("[AppUserService] Refresh token has expired");
                throw new Exception("Refresh token has expired");
            }

            var tokens = await GenratTokens(user);

            if (tokens == null)
            {
                Console.WriteLine("[AppUserService] GenratTokens returned null");
                throw new Exception("Failed to generate tokens");
            }

            Console.WriteLine($"[AppUserService] Generated new tokens successfully. New RefreshToken: {tokens.RefreshToken}");
            return tokens;
        }



        public async Task<Tokens> GenratTokens(AppUser user)
        {
            Tokens tokens = new Tokens();
            tokens.RefreshToken = Guid.NewGuid().ToString("N");
            tokens.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(30);


            user.RefreshToken = tokens.RefreshToken;
            user.RefreshTokenExpirationTime = tokens.RefreshTokenExpirationTime;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new Exception("Failed to update user with refresh token");
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName ?? "Not Found"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email ?? "Not Found"));
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? "your-secret-key"));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"] ?? "your-issuer",
                audience: _configuration["JWT:Audience"] ?? "your-audience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
                );

            tokens.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            tokens.AccessTokenExpirationTime = DateTime.UtcNow.AddHours(1);

            return tokens;
        }

        public async Task CheckSecreatKey(CheckAccess secreatKey)
        {


            var user = await _userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
              throw new KeyNotFoundException();

            var result = await _userManager.CheckPasswordAsync(user, secreatKey.pass);

            if (!result)
            {
                throw new Exception("Wrong Pass");

            }

        }

        public async Task Register(RegisterD registerD)
        {
            
            var accountCount = await _userManager.Users.CountAsync();
            
            if (accountCount >1)
                throw new Exception("Account count is greater than 1");
            
            AppUser appUser = new AppUser
            {
                UserName = registerD.UserName,
                Email=registerD.Email
            };
            var result=  await _userManager.CreateAsync(appUser,registerD.Password);

            if(!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            }

        }

        public async Task ChangePassword(ChangePassword changePassword)
        {
            var principal = httpContextAccessor.HttpContext?.User;
            if (principal == null)
                throw new KeyNotFoundException("المستخدم غير مسجل");

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = !string.IsNullOrEmpty(userId)
                ? await _userManager.FindByIdAsync(userId)
                : await _userManager.GetUserAsync(principal);

            if (user == null)
            {
                var username = principal.Identity?.Name;
                if (!string.IsNullOrEmpty(username))
                    user = await _userManager.FindByNameAsync(username);
            }

            if (user == null)
                throw new KeyNotFoundException("تعذر العثور على حساب المستخدم");

            var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }
    }
}
