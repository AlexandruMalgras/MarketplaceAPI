using Marketplace.Executors;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Marketplace.Configurations
{
    public class UserActionLoggingActionFilter : IAsyncActionFilter
    {
        private readonly UserActionsQueryExecutor userActionsQueryExecutor;
        private readonly UsersQueryExecutor usersQueryExecutor;

        public UserActionLoggingActionFilter(UserActionsQueryExecutor userActionsQueryExecutor, UsersQueryExecutor usersQueryExecutor)
        {
            this.userActionsQueryExecutor = userActionsQueryExecutor;
            this.usersQueryExecutor = usersQueryExecutor;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            var userIdClaim = executedContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var user = await usersQueryExecutor.ReadUserAsync(userIdClaim.Value);
                if (user != null)
                {
                    var result = executedContext.Exception == null ? "Success" : "Failure";

                    await userActionsQueryExecutor.CreateUserActionAsync(
                        user,
                        executedContext.HttpContext.Request.Method,
                        executedContext.HttpContext.Request.Path,
                        result);
                }
            }
        }
    }
}
