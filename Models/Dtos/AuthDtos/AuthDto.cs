namespace TheBookClub.Models.Dtos.AuthDtos
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
    }

    public class ConfirmEmailRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public Guid UserId { get; set; } 
        public string Email { get; set; }
        public string Token { get; set; } 
        public DateTime TokenExpiryTime { get; set; } 
        public string? RefreshToken { get; set; } 
        public DateTime? RefreshTokenExpiryTime { get; set; } 
    }

    public class ResendVerificationRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class PasswordResetRequest
    {
        public string Email { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}