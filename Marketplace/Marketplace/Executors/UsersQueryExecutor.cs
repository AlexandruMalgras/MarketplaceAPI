using Marketplace.Configurations;
using Marketplace.Models;
using Marketplace.TransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Marketplace.Executors
{
    public class UsersQueryExecutor
    {
        private readonly UserManager<Users> userManager;

        public UsersQueryExecutor(UserManager<Users> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Users?> ReadUser(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> DeleteUser(Users user)
        {           
            return await userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUser(Users user, UpdateUserTransferObject updateUser)
        {
            user.Email = updateUser.Email ?? user.Email;
            user.PhoneNumber = updateUser.PhoneNumber ?? user.PhoneNumber;
            user.FirstName = updateUser.FirstName ?? user.FirstName;
            user.LastName = updateUser.LastName ?? user.LastName;

            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUserPassword(Users user, string password)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            return await userManager.ResetPasswordAsync(user, token, password);
        }
    }
}
