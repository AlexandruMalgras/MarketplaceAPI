using Marketplace.Executors;
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
            var users = await usersQueryExecutor.ReadUsersAsync();

            if (users != null)
            {
                return Ok(new { users = users });
            }

            return NotFound("No users were found.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound("No user with id " + id + " exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                var result = await usersQueryExecutor.DeleteUserAsync(user);

                return NoContent();
            }

            return NotFound("No user with id " + id + " exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UpdateUserTransferObject transfer)
        {
            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                var result = await usersQueryExecutor.UpdateUserAsync(user, transfer);

                user = await usersQueryExecutor.ReadUserAsync(id);

                return Ok(user);
            }

            return NotFound("No user with id " + id + " exists.");
        }
    }
}
