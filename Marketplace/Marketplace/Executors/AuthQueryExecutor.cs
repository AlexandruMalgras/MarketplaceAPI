using Marketplace.Models;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Executors
{
    public class AuthQueryExecutor
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;

        public AuthQueryExecutor(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUser(Users user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }

        public async Task<SignInResult> LoginUser(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await signInManager.PasswordSignInAsync(user, password, false, false);
        }
    }
}
