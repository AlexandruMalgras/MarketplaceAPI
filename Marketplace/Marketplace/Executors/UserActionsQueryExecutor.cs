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

        public async Task<IdentityResult> CreateUserActionAsync(Users user, string method, string uri, int statusCode, string? exception)
        {
            user.UserActions.Add(new UserActions
            {
                Method = method,
                Uri = uri,
                StatusCode = statusCode,
                Exception = exception,
                ExecutionTime = DateTime.Now
            });

            return await userManager.UpdateAsync(user);
        }
    }
}
