using Marketplace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Configurations
{
    public class InitialDataSeeder
    {
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public InitialDataSeeder(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task SeedData()
        {
            string[] roles = new string[] { "Admin", "User" };

            foreach (string role in roles)
            {
                if (!await this.roleManager.RoleExistsAsync(role))
                {
                    await this.roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminUser = new Users { UserName = "AdminUser", Email = "admin@example.com", CreationTime = DateTime.Now };
            var result = await this.userManager.CreateAsync(adminUser, "SecurePassword123!");

            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
