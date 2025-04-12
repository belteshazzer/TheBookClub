using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheBookClub.Services.AuthServices.IAuthServices;

namespace TheBookClub.Services.AuthServices.AuthServices
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleService(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<bool> AddRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                throw new InvalidOperationException($"The role '{roleName}' already exists.");
            }

            var role = new IdentityRole<Guid>(roleName);
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create role '{roleName}'. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result.Succeeded;
        }

        public async Task<bool> RemoveRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new KeyNotFoundException($"The role '{roleName}' does not exist.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to delete role '{roleName}'. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result.Succeeded;
        }

        public async Task<IList<string>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Select(r => r.Name)
                .Where(name => name != null)
                .Cast<string>()
                .ToListAsync();

            if (roles == null || !roles.Any())
            {
                throw new InvalidOperationException("No roles found.");
            }

            return roles;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
            }

            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityRole<Guid>> GetRoleByIdAsync(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString()) ?? throw new KeyNotFoundException($"The role with ID '{roleId}' does not exist.");
            return role;
        }
    }
}