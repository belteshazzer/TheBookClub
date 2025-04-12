using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using TheBookClub.Models.Entities;
using TheBookClub.Services.AuthServices.IAuthServices;

namespace TheBookClub.Services.AuthServices.AuthServices
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserRoleService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AddUserToRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (user == null || role == null)
            {
                throw new ArgumentException("User or role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (user == null || role == null)
            {
                throw new ArgumentException("User or role not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsUserInRoleAsync(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (user == null || role == null)
            {
                throw new ArgumentException("User or role not found.");
            }

            return await _userManager.IsInRoleAsync(user, role.Name);
        }
    }
}