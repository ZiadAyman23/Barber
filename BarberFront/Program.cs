using BarberFront.Components;
using BarberFront.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BarberFront
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Authentication & Authorization Services (Server-side + Blazor)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"] ?? "https://localhost:7074",
                    ValidAudience = builder.Configuration["JWT:Audience"] ?? "https://localhost:7106",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "dkjvzd;dfj;adfjnvkjadnkhdfb6d5454fds4G545sDG5sds4df5s54bDFbf4b61er8grdafv65f16f1ds5vad165erf515F4F"))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["AccessToken"];
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        context.Response.Redirect("/login");
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddTransient<JwtAuthorizationHandler>();

            // Auth client — no JwtAuthorizationHandler to avoid circular token refresh loops
            builder.Services.AddHttpClient("Auth", client => { client.BaseAddress = new Uri("https://localhost:7074/"); });

            // API client — all requests go through JWT handler (adds token, handles 401 refresh)
            builder.Services.AddHttpClient("API", client => { client.BaseAddress = new Uri("https://localhost:7074/"); })
                .AddHttpMessageHandler<JwtAuthorizationHandler>();
            builder.Services.AddScoped<ItemService>();
            builder.Services.AddScoped<BarberService>();
            builder.Services.AddScoped<ExpenseService>();
            builder.Services.AddScoped<DeductionService>();
            builder.Services.AddScoped<AttentanceService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<SalaryService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<MonthelyReportServ>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
