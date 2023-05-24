using Marketplace.Executors;
using Marketplace.TransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Marketplace.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private UsersQueryExecutor usersQueryExecutor;

        public UsersController(UsersQueryExecutor usersQueryExecutor)
        {
            this.usersQueryExecutor = usersQueryExecutor;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserAsync()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {
                return StatusCode(404, "No user with id " + id + " exists.");
            }

            return StatusCode(200, user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserTransferObject transfer)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {
                return StatusCode(404, "No user with id " + id + " exists.");
            }

            var result = await usersQueryExecutor.UpdateUserAsync(user, transfer);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(t => t.Description);
                return StatusCode(500, errors);
            }

            user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {
                return StatusCode(500, "An error occured while retrieving the user.");
            }

            return StatusCode(200, user);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdateUserPasswordAsync([FromBody] UpdateUserPasswordTransferObject transfer)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {
                return StatusCode(404, "No user with id " + id + " exists.");
            }

            var result = await usersQueryExecutor.UpdateUserPasswordAsync(user, transfer.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(t => t.Description);
                return StatusCode(400, errors);
            }

            return StatusCode(204);
        }
    }
}
