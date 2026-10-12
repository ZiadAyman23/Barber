
global using Core.IRepository;
global using Core.IService;
global using Core.Models;
global using Infrastructer.Context;
global using Infrastructer.Repository;
global using Infrastructer.Service;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
using Microsoft.AspNetCore.RateLimiting;

namespace Barbershop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"] ?? "your-secret-key"))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies["AccessToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazor", policy =>
                {
                    policy.WithOrigins("https://localhost:7106")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            builder.Services.AddRateLimiter(options =>
            {
                options.AddSlidingWindowLimiter("Login", limiter =>
                {
                    limiter.PermitLimit = 5;
                    limiter.Window = TimeSpan.FromMinutes(5);
                    limiter.QueueLimit = 0;
                    limiter.SegmentsPerWindow = 10;
                });
                
            });
            // Add services for dependency injection
            builder.Services.AddScoped<IAppUserService, AppUserService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IBarberService, BarberService>();
            builder.Services.AddScoped<IExpenseService, ExpenseService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<IDeductionService, DeductionServes>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ISalaryService, SalaryService>();
            builder.Services.AddScoped<IMonthelyReportService, MonthelyReportService>();

            // Add Repository 
            builder.Services.AddScoped<IItemRepo, ItemRepo>();
            builder.Services.AddScoped<IBaeberRepo, BarberRepo>();
            builder.Services.AddScoped<IExpenseRepo, ExpeseRepo>();
            builder.Services.AddScoped<IAttendanceRepo, AttendanceRepo>();
            builder.Services.AddScoped<IDeductionRepo, DeductionRepo>();
            builder.Services.AddScoped<IOrderRepo,OrderRepo>();
            builder.Services.AddScoped<ISalaryRepo, SalaryRepo>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseCors("AllowBlazor");
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
