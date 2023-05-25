using Marketplace.Executors;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Marketplace.Configurations
{
    public class UserActionLoggingActionFilter : IAsyncResultFilter
    {
        private readonly UserActionsQueryExecutor userActionsQueryExecutor;
        private readonly UsersQueryExecutor usersQueryExecutor;

        public UserActionLoggingActionFilter(UserActionsQueryExecutor userActionsQueryExecutor, UsersQueryExecutor usersQueryExecutor)
        {
            this.userActionsQueryExecutor = userActionsQueryExecutor;
            this.usersQueryExecutor = usersQueryExecutor;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var executedContext = await next();

            var userIdClaim = executedContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                var user = await usersQueryExecutor.ReadUserAsync(userIdClaim.Value);

                if (user != null)
                {
                    await userActionsQueryExecutor.CreateUserActionAsync(
                        user,
                        executedContext.HttpContext.Request.Method,
                        executedContext.HttpContext.Request.Path,
                        executedContext.HttpContext.Response.StatusCode,
                        executedContext.Exception?.StackTrace
                        );
                }
            }
        }
    }
}
