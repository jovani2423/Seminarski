using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentACar.BLL.Services;
using RentACar.DAL.Identity;
using RentACar.DAL.Repositories;

namespace RentACar.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogic(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string is missing.");
            }

            services.AddDbContext<ApplicationIdentityDbContext>(
                options =>
                    options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Zakljucavanje naloga nakon neuspesnih pokusaja
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Pravila za lozinke
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;

                // Pravila za email
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<CarRepository>();
            services.AddScoped<CarService>();
            services.AddScoped<AccountService>();
            services.AddScoped<ReservationRepository>();
            services.AddScoped<ReservationService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<DashboardService>();
            services.AddScoped<CarCategoryRepository>();
            services.AddScoped<CarCategoryService>();
            services.AddScoped<BranchRepository>();
            services.AddScoped<BranchService>();
            services.AddScoped<MaintenanceRecordRepository>();
            services.AddScoped<MaintenanceRecordService>();

            return services;
        }
    }
}
