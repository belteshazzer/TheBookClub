namespace TheBookClub.Services.AuthServices.IAuthServices
{
    public interface IUserRoleService
    {
        Task<bool> AddUserToRoleAsync(Guid userId, Guid roleId);
        Task<bool> RemoveUserFromRoleAsync(Guid userId, Guid roleId);
        Task<IList<string>> GetUserRolesAsync(Guid userId);
        Task<bool> IsUserInRoleAsync(Guid userId, Guid roleId);
    }
}