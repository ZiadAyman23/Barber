using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;
using Core.ModelView;
using System.Net.Http.Json;
using Core.DTO;

namespace BarberFront.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        // Cached in-memory — survives re-renders within the same SignalR circuit
        private ClaimsPrincipal? _currentUser;

        // Prevents concurrent refresh calls
        private static readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);
        private static DateTime _lastRefreshAttempt = DateTime.MinValue;
        private static readonly TimeSpan _refreshCooldown = TimeSpan.FromSeconds(10);

        public CustomAuthenticationStateProvider(
            IJSRuntime jsRuntime,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory)
        {
            _jsRuntime = jsRuntime;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // If we already have a valid authenticated user cached, return it immediately (no HTTP calls)
            if (_currentUser != null && _currentUser.Identity?.IsAuthenticated == true)
            {
                // Validate the cached token is not expired
                var expClaim = _currentUser.FindFirst("exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out var expUnix))
                {
                    var expTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                    if (expTime > DateTime.UtcNow.AddSeconds(30))
                    {
                        // Token still valid — return cached state, no network call
                        return new AuthenticationState(_currentUser);
                    }
                    // Token expired — fall through to refresh
                }
                else
                {
                    return new AuthenticationState(_currentUser);
                }
            }

            // --- Try to get token ---
            string? token = GetTokenFromHttpContext();

            // Fallback to JS Interop (only works after Blazor circuit is established)
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await GetTokenFromJsAsync();
            }

            // If we have a token, validate it
            if (!string.IsNullOrWhiteSpace(token))
            {
                var (isValid, isExpired) = ValidateToken(token);

                if (isValid && !isExpired)
                {
                    return BuildAuthState(token);
                }

                if (isExpired)
                {
                    // Try refresh only if token is expired
                    var newToken = await TryRefreshTokenAsync();
                    if (!string.IsNullOrWhiteSpace(newToken))
                        return BuildAuthState(newToken);
                }
            }
            else
            {
                // No access token at all — try refresh (e.g. after browser restart, cookie present but access token missing)
                var newToken = await TryRefreshTokenAsync();
                if (!string.IsNullOrWhiteSpace(newToken))
                    return BuildAuthState(newToken);
            }

            // Not authenticated
            _currentUser = _anonymous;
            return new AuthenticationState(_anonymous);
        }

        private string? GetTokenFromHttpContext()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                    return httpContext.Request.Cookies["AccessToken"];
            }
            catch { }
            return null;
        }

        private async Task<string?> GetTokenFromJsAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("storage.getAccessToken");
            }
            catch { }
            return null;
        }

        private (bool isValid, bool isExpired) ValidateToken(string token)
        {
            try
            {
                var claims = ParseClaimsFromJwt(token);
                var expClaim = claims.FirstOrDefault(c => c.Type == "exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out var expUnix))
                {
                    var expTime = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                    return (true, expTime < DateTime.UtcNow);
                }
                return (true, false);
            }
            catch
            {
                return (false, false);
            }
        }

        private AuthenticationState BuildAuthState(string token)
        {
            try
            {
                var claims = ParseClaimsFromJwt(token);
                var identity = new ClaimsIdentity(claims, "jwt");
                _currentUser = new ClaimsPrincipal(identity);
                return new AuthenticationState(_currentUser);
            }
            catch
            {
                _currentUser = _anonymous;
                return new AuthenticationState(_anonymous);
            }
        }

        private async Task<string?> TryRefreshTokenAsync()
        {
            // Cooldown to prevent hammering the refresh endpoint
            if (DateTime.UtcNow - _lastRefreshAttempt < _refreshCooldown)
                return null;

            if (!await _refreshLock.WaitAsync(0))
                return null; // Another refresh is in progress

            try
            {
                _lastRefreshAttempt = DateTime.UtcNow;

                string? refreshToken = GetRefreshTokenFromHttpContext();

                if (string.IsNullOrEmpty(refreshToken))
                    refreshToken = await GetRefreshTokenFromJsAsync();

                if (string.IsNullOrEmpty(refreshToken))
                    return null;

                var client = _httpClientFactory.CreateClient("Auth");
                var response = await client.PostAsJsonAsync("api/Account/refresh-token",
                    new RefreshTokenRequest { RefreshToken = refreshToken });

                if (response.IsSuccessStatusCode)
                {
                    var tokens = await response.Content.ReadFromJsonAsync<Tokens>();
                    if (tokens != null && !string.IsNullOrEmpty(tokens.AccessToken))
                    {
                        // Save new tokens to JS/cookie storage
                        try
                        {
                            await _jsRuntime.InvokeVoidAsync("storage.setAccessToken", tokens.AccessToken);
                            await _jsRuntime.InvokeVoidAsync("storage.setRefreshToken", tokens.RefreshToken);
                        }
                        catch { }

                        // Reset cooldown on success
                        _lastRefreshAttempt = DateTime.MinValue;
                        return tokens.AccessToken;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private string? GetRefreshTokenFromHttpContext()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                    return httpContext.Request.Cookies["RefreshToken"];
            }
            catch { }
            return null;
        }

        private async Task<string?> GetRefreshTokenFromJsAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("storage.getRefreshToken");
            }
            catch { }
            return null;
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);
            _lastRefreshAttempt = DateTime.MinValue; // Reset cooldown
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        }

        public void MarkUserAsLoggedOut()
        {
            _currentUser = _anonymous;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var parts = jwt.Split('.');
            if (parts.Length < 2) return claims;

            var payload = parts[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);

            using var jsonDocument = JsonDocument.Parse(jsonBytes);
            foreach (var element in jsonDocument.RootElement.EnumerateObject())
            {
                var key = element.Name;
                var value = element.Value;

                var claimType = key switch
                {
                    "nameid" or "sub" => ClaimTypes.NameIdentifier,
                    "unique_name" or "name" => ClaimTypes.Name,
                    "email" => ClaimTypes.Email,
                    "role" or "roles" => ClaimTypes.Role,
                    _ => key
                };

                if (value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in value.EnumerateArray())
                    {
                        var valStr = item.ValueKind == JsonValueKind.String ? item.GetString() : item.GetRawText();
                        claims.Add(new Claim(claimType, valStr ?? string.Empty));
                    }
                }
                else
                {
                    var valStr = value.ValueKind == JsonValueKind.String ? value.GetString() : value.GetRawText();
                    claims.Add(new Claim(claimType, valStr ?? string.Empty));
                }
            }

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            base64 = base64.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
