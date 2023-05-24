using Marketplace.Configurations;
using Marketplace.Models;
using Marketplace.TransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Executors
{
    public class UsersQueryExecutor
    {
        private readonly UserManager<Users> userManager;
        private readonly DatabaseConfiguration context;

        public UsersQueryExecutor(UserManager<Users> userManager, DatabaseConfiguration databaseConfiguration)
        {
            this.userManager = userManager;
            this.context = databaseConfiguration;
        }

        public async Task<Users?> ReadUserAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<List<Users>> ReadUsersAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> DeleteUserAsync(Users user)
        {
            var userWithActions = await context.Users.Include(u => u.UserActions).SingleOrDefaultAsync(u => u.Id == user.Id);

            if (userWithActions != null)
            {
                context.UserActions.RemoveRange(userWithActions.UserActions);

                await context.SaveChangesAsync();
            }

            return await userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(Users user, UpdateUserTransferObject updateUser)
        {
            user.Email = updateUser.Email ?? user.Email;
            user.PhoneNumber = updateUser.PhoneNumber ?? user.PhoneNumber;
            user.FirstName = updateUser.FirstName ?? user.FirstName;
            user.LastName = updateUser.LastName ?? user.LastName;

            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUserPasswordAsync(Users user, string password)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            return await userManager.ResetPasswordAsync(user, token, password);
        }
    }
}
