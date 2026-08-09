using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Core.ModelView;

namespace BarberFront.Services
{
    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthorizationHandler(
            IJSRuntime jsRuntime, 
            IHttpClientFactory httpClientFactory,
            NavigationManager navigationManager,
            AuthenticationStateProvider authStateProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _jsRuntime = jsRuntime;
            _httpClientFactory = httpClientFactory;
            _navigationManager = navigationManager;
            _authStateProvider = authStateProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? accessToken = null;

            // 1. Try to get token from HttpContext
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    accessToken = httpContext.Request.Cookies["AccessToken"];
                }
            }
            catch
            {
                // HttpContext not available
            }

            // 2. Try to get token from storage via JS
            if (string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    accessToken = await _jsRuntime.InvokeAsync<string>("storage.getAccessToken");
                }
                catch
                {
                    // JS Interop not available (e.g. pre-rendering)
                }
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                // Also set Cookie header for the backend OnMessageReceived event
                request.Headers.Add("Cookie", $"AccessToken={accessToken}");
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // If the request itself was login or refresh-token, don't trigger background refresh loop
                var requestPath = request.RequestUri?.AbsolutePath ?? "";
                if (requestPath.Contains("login", StringComparison.OrdinalIgnoreCase) ||
                    requestPath.Contains("refresh-token", StringComparison.OrdinalIgnoreCase))
                {
                    return response;
                }

                // Token might be expired, try refreshing it
                var isRefreshed = await TryRefreshTokenAsync();
                if (isRefreshed)
                {
                    accessToken = null;
                    try
                    {
                        accessToken = await _jsRuntime.InvokeAsync<string>("storage.getAccessToken");
                    }
                    catch {}

                    if (string.IsNullOrEmpty(accessToken))
                    {
                        try
                        {
                            var httpContext = _httpContextAccessor.HttpContext;
                            if (httpContext != null)
                            {
                                accessToken = httpContext.Request.Cookies["AccessToken"];
                            }
                        }
                        catch {}
                    }

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        request.Headers.Remove("Cookie");
                        request.Headers.Add("Cookie", $"AccessToken={accessToken}");

                        response.Dispose();
                        response = await base.SendAsync(request, cancellationToken);
                    }
                }
            }

            return response;
        }

        private async Task<bool> TryRefreshTokenAsync()
        {
            try
            {
                string? refreshToken = null;
                try
                {
                    var httpContext = _httpContextAccessor.HttpContext;
                    if (httpContext != null)
                    {
                        refreshToken = httpContext.Request.Cookies["RefreshToken"];
                    }
                }
                catch {}

                if (string.IsNullOrEmpty(refreshToken))
                {
                    try
                    {
                        refreshToken = await _jsRuntime.InvokeAsync<string>("storage.getRefreshToken");
                    }
                    catch {}
                }

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return false;
                }

                var client = _httpClientFactory.CreateClient("Auth");

                var response = await client.PostAsJsonAsync("api/Account/refresh-token", new RefreshTokenRequest { RefreshToken = refreshToken });
                if (response.IsSuccessStatusCode)
                {
                    var tokens = await response.Content.ReadFromJsonAsync<Tokens>();
                    if (tokens != null && !string.IsNullOrEmpty(tokens.AccessToken))
                    {
                        await _jsRuntime.InvokeVoidAsync("storage.setAccessToken", tokens.AccessToken);
                        await _jsRuntime.InvokeVoidAsync("storage.setRefreshToken", tokens.RefreshToken);

                        if (_authStateProvider is CustomAuthenticationStateProvider customProvider)
                        {
                            customProvider.MarkUserAsAuthenticated(tokens.AccessToken);
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task LogOutAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("storage.clear");
            }
            catch
            {
            }

            if (_authStateProvider is CustomAuthenticationStateProvider customProvider)
            {
                customProvider.MarkUserAsLoggedOut();
            }

            // Safely navigate in background to handle uninitialized RemoteNavigationManager during startup
            _ = Task.Run(async () =>
            {
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        _navigationManager.NavigateTo("/login");
                        return;
                    }
                    catch (InvalidOperationException)
                    {
                        await Task.Delay(200);
                    }
                    catch
                    {
                        return;
                    }
                }
            });
        }
    }
}
