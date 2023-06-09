﻿using Marketplace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Marketplace.Executors
{
    public class AuthQueryExecutor
    {
        private readonly UserManager<Users> userManager;
        private readonly SignInManager<Users> signInManager;
        private readonly IConfiguration config;

        public AuthQueryExecutor(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            config = new ConfigurationBuilder().AddJsonFile(Environment.CurrentDirectory + "/JWTBearer.json").Build();
        }

        public async Task<IdentityResult> CreateUserAsync(Users user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await this.userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }

        public async Task<SignInResult> LoginUserAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await signInManager.PasswordSignInAsync(user, password, false, false);
        }

        public async Task<JwtSecurityToken> GenerateJSONWebTokenAsync(string username)
        {
            var user = await userManager.FindByNameAsync(username);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTBearer:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRole = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            claims.AddRange(userRole.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(config["JWTBearer:Issuer"],
                config["JWTBearer:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return token;
        }
    }
}
