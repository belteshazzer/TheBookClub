using Microsoft.AspNetCore.Identity;

namespace TheBookClub.Services.AuthServices.IAuthServices
{
    public interface IRoleService
    {
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> RemoveRoleAsync(string roleName);
        Task<IList<string>> GetAllRolesAsync();
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityRole<Guid>> GetRoleByIdAsync(Guid roleId);
    }
}