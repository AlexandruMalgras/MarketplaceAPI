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
        private UsersQueryExecutor queryExecutor;

        public AdminUsersController(UsersQueryExecutor queryExecutor)
        {
            this.queryExecutor = queryExecutor;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await queryExecutor.ReadUsersAsync();

            if (users != null)
            {
                return Ok(new { users = users });
            }

            return NotFound("No users were found.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await queryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound("No user with id " + id + " exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            var user = await queryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                var result = await queryExecutor.DeleteUserAsync(user);

                return NoContent();
            }

            return NotFound("No user with id " + id + " exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UpdateUserTransferObject transfer)
        {
            var user = await queryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                var result = await queryExecutor.UpdateUserAsync(user, transfer);

                user = await queryExecutor.ReadUserAsync(id);

                return Ok(user);
            }

            return NotFound("No user with id " + id + " exists.");
        }
    }
}
