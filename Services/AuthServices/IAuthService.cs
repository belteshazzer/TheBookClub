
using TheBookClub.Models.Dtos.AuthDtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.AuthServices
{
    public interface IAuthService
    {
        Task<User> RegisterUserAsync(User user, string password);
        Task VerifyEmailAsync(string email, string token);
        Task<bool> ResendVerificationEmailAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<bool> SendPasswordResetEmailAsync(string email);
        Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<LoginResponse> LoginUserAsync(string email, string password);
        Task LogoutUserAsync();
    }
}