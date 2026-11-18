using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RentACar.DAL.Identity;

namespace RentACar.BLL
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            const string roleName = "Admin";
            const string adminEmail = "jovan.ilijev@gmail.com";

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(roleName));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                throw new InvalidOperationException(
                    "Administratorski nalog nije pronađen.");
            }

            if (!await userManager.IsInRoleAsync(adminUser, roleName))
            {
                await userManager.AddToRoleAsync(adminUser, roleName);
            }
        }
    }
}
