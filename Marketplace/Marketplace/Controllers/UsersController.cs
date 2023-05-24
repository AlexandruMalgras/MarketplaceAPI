using Marketplace.Executors;
using Marketplace.Models;
using Marketplace.TransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound("No user with id " + id + " exists.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                var result = await usersQueryExecutor.DeleteUserAsync(user);

                return NoContent();
            }
            
            return NotFound("No user with id " + id + " exists.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserTransferObject transfer)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                var result = await usersQueryExecutor.UpdateUserAsync(user, transfer);

                user = await usersQueryExecutor.ReadUserAsync(id);

                return Ok(user);
            }

            return NotFound("No user with id " + id + " exists.");
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdateUserPasswordAsync([FromBody] UpdateUserPasswordTransferObject transfer)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await usersQueryExecutor.ReadUserAsync(id);

            if (user != null)
            {
                var result = await usersQueryExecutor.UpdateUserPasswordAsync(user, transfer.Password);

                if (result.Succeeded)
                {
                    return NoContent();
                }

                var errors = result.Errors.Select(t => t.Description);
                return BadRequest(errors);
            }

            return NotFound("No user with id " + id + " exists.");
        }
    }
}
