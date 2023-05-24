using Marketplace.Executors;
using Marketplace.Models;
using Marketplace.TransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace Marketplace.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUsersController : ControllerBase
    {
        private UsersQueryExecutor usersQueryExecutor;

        public AdminUsersController(UsersQueryExecutor usersQueryExecutor)
        {
            this.usersQueryExecutor = usersQueryExecutor;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await usersQueryExecutor.ReadUsersAsync() ?? new List<Users>();

            return StatusCode(200, new { users = users });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {
                return StatusCode(404, "No user with id " + id + " exists.");
            }

            return StatusCode(200, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {

                return StatusCode(404, "No user with id " + id + " exists.");
            }

            var result = await usersQueryExecutor.DeleteUserAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return StatusCode(500, errors);
            }

            return StatusCode(204);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UpdateUserTransferObject transfer)
        {
            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {
                return StatusCode(404, "No user with id " + id + " exists.");
            }

            var result = await usersQueryExecutor.UpdateUserAsync(user, transfer);

            user = await usersQueryExecutor.ReadUserAsync(id);

            if (user == null)
            {
                return StatusCode(500, "An error occured while retrieving the user.");
            }

            return StatusCode(200, user);
        }
    }
}
