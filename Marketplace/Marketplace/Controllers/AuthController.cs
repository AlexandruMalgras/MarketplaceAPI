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
        public async Task<IActionResult> PostUser([FromBody] CreateUserTransferObject transfer)
        {
            var newUser = new Users { UserName = transfer.UserName, Email = transfer.Email, PhoneNumber = transfer.PhoneNumber, FirstName = transfer.FirstName, LastName = transfer.LastName };

            var result = await authQueryExecutor.CreateUser(newUser, transfer.Password);

            if (result.Succeeded)
            {
                return Created("/api/Auth/" + newUser.Id, usersQueryExecutor.ReadUser(newUser.Id));
            }
            else
            {
                var errors = result.Errors.Select(t => t.Description);
                return BadRequest(errors);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserTransferObject transfer)
        {
            var result = await authQueryExecutor.LoginUser(transfer.UserName, transfer.Password);

            if (result.Succeeded)
            {
                var token = await authQueryExecutor.GenerateJSONWebToken(transfer.UserName);
                var tokenHandler = new JwtSecurityTokenHandler();
                var serializedToken = tokenHandler.WriteToken(token);

                return Ok(new { JsonWebToken = serializedToken });
            }

            return BadRequest("Incorrect username or password.");
        }
    }
}
