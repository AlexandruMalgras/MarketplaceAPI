using Marketplace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Executors
{
    public class UserActionsQueryExecutor
    {
        private readonly UserManager<Users> userManager;

        public UserActionsQueryExecutor(UserManager<Users> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserActionAsync(Users user, string method, string uri, string result)
        {
            user.UserActions.Add(new UserActions
            {
                Method = method,
                Uri = uri,
                Result = result,
                ExecutionTime = DateTime.Now
            });

            return await userManager.UpdateAsync(user);
        }
    }
}
