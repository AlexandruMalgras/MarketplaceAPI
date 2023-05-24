using Marketplace.Executors;
using Marketplace.Models;
using Marketplace.TransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private AuthQueryExecutor authQueryExecutor;
        private UsersQueryExecutor usersQueryExecutor;

        public AuthController(AuthQueryExecutor authQueryExecutor, UsersQueryExecutor usersQueryExecutor)
        {
            this.authQueryExecutor = authQueryExecutor;
            this.usersQueryExecutor = usersQueryExecutor;
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostUserAsync([FromBody] CreateUserTransferObject transfer)
        {
            var newUser = new Users { UserName = transfer.UserName,
                Email = transfer.Email,
                PhoneNumber = transfer.PhoneNumber,
                FirstName = transfer.FirstName,
                LastName = transfer.LastName,
                CreationTime = DateTime.Now };

            var result = await authQueryExecutor.CreateUserAsync(newUser, transfer.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(t => t.Description);
                return StatusCode(400, errors);
            }

            var user = await usersQueryExecutor.ReadUserAsync(newUser.Id);

            if (user == null)
            {
                return StatusCode(500, "An error occured while retrieving the user.");
            }

            return StatusCode(201, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginUserTransferObject transfer)
        {
            var result = await authQueryExecutor.LoginUserAsync(transfer.UserName, transfer.Password);

            if (!result.Succeeded)
            {
                return StatusCode(400, "Incorrect username or password.");
            }

            var token = await authQueryExecutor.GenerateJSONWebTokenAsync(transfer.UserName);
            var tokenHandler = new JwtSecurityTokenHandler();
            var serializedToken = tokenHandler.WriteToken(token);

            return StatusCode(200, new { JsonWebToken = serializedToken });
        }
    }
}
