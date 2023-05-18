using Marketplace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Configurations
{
    public class InitialDataSeeder
    {
        private readonly DatabaseConfiguration context;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public InitialDataSeeder(DatabaseConfiguration context, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedData()
        {
            await this.context.Database.EnsureCreatedAsync();

            string[] roles = new string[] { "Admin", "User" };

            foreach (string role in roles)
            {
                if (!await this.roleManager.RoleExistsAsync(role))
                {
                    await this.roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = new Users { UserName = "AdminUser", Email = "admin@example.com" };
            var result = await this.userManager.CreateAsync(adminUser, "SecurePassword123!");

            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
