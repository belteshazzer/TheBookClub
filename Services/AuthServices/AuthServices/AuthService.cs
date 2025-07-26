using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TheBookClub.Common;
using TheBookClub.Common.EmailSender;
using TheBookClub.Common.Exceptions;
using TheBookClub.Models.Dtos.AuthDtos;
using TheBookClub.Models.Entities;
using TheBookClub.Services.AuthServices.IAuthServices;
using System.Text.Encodings.Web;

namespace TheBookClub.Services.AuthServices.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UrlEncoder _urlEncoder = UrlEncoder.Default;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<Guid>> roleManager, IEmailSender emailSender, IConfiguration configuration, ILogger<AuthService> logger,IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<User> RegisterUserAsync(User user, string password)
        {
            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            { 
                throw new ConflictException("A user with this email already exists.");
            }

            // Ensure username is set
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                user.UserName = user.Email; 
            }

            // Attempt to create the user
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); // Assign default role
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _logger.LogInformation("token: {token}", token);
                await SendConfirmationEmail(user.Email, token);
                return user;
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("User registration failed: {Errors}", errors);
                throw new Exception("User registration failed: " + errors);
            }
        }

        private async Task SendConfirmationEmail(string email, string token)
        {
            var confirmationLink = $"http://localhost:5039/api/Auth/confirm-email?email={email}&token={Uri.EscapeDataString(token)}";
            _logger.LogInformation("confirmationLink: {confirmationLink}", confirmationLink);

            var subject = "Password Reset Request";
            var body = string.Format(EmailTemplates.ConfirmEmailTemplate, confirmationLink);
            var emailSent = await _emailSender.SendEmail(email, subject, body);
            if (!emailSent)
            {
                throw new Exception("Failed to send confirmation email, please try again");
            }
        }

       public async Task<bool> VerifyEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");
            if (user.EmailConfirmed)
            {
                throw new Exception("Email is already confirmed"); 
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Email confirmation failed: {Errors}", errors);
                throw new Exception("Email confirmation failed: " + errors);
            }

            var role = await _roleManager.FindByNameAsync("User") ?? throw new NotFoundException("Role not found");
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                throw new Exception("Role name is invalid");
            }
            await _userManager.AddToRoleAsync(user, role.Name);

            return true;
        }

        public async Task<LoginResponse> LoginUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User not found");
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {               
                var token = await GenerateJwtToken(user);
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                var loginResponse = new LoginResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Token = token,
                    TokenExpiryTime = DateTime.UtcNow.AddHours(1),
                    RefreshToken = user.RefreshToken, 
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime 
                }; 
                return loginResponse;
            }
            else if (await _userManager.IsLockedOutAsync(user))
            {
                throw new AccessDeniedException("User account is locked out. Please contact support."); 
            }
            else if (result.IsNotAllowed)
            {
                throw new UnauthorizedException("User is not allowed to login. Please verify your email first.");
            }
            else 
            {
                throw new BadRequestException("Invalid login attempt. Please check your credentials."); 
            }
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var role = await _userManager.GetRolesAsync(user) ?? throw new NotFoundException("Role not found");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            claims.AddRange(role.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(string key, string qrCodeUrl)> Enable2FAAsync()
        {
            // Get the current user from the HTTP context
            var userId = TokenHelper.GetUserId(_httpContextAccessor.HttpContext);
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new NotFoundException("User not found");

            // Retrieve the current authenticator key
            var key = await _userManager.GetAuthenticatorKeyAsync(user);
            _logger.LogInformation("Authenticator key: {Key}", key);

            // If the key is null or empty, reset and generate a new authenticator key
            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            // Generate the QR code URL for the authenticator app
            var qrCodeUrl = $"otpauth://totp/{Uri.EscapeDataString("TheBookClub")}:{Uri.EscapeDataString(user.Email)}?secret={key}&issuer={Uri.EscapeDataString("TheBookClub")}&digits=6";
            _logger.LogInformation("QR Code URL: {QrCodeUrl}", qrCodeUrl);

            return (key, qrCodeUrl);
        }

        public async Task<bool> Verify2FACodeAsync(string code)
        {
            // Get the current user from the HTTP context
            var userId = TokenHelper.GetUserId(_httpContextAccessor.HttpContext);
            var user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new NotFoundException("User not found");

            // Verify the provided 2FA code
            var result = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, code);
            if (!result)
            {
                throw new UnauthorizedException("Invalid 2FA code. Please try again.");
            }

            user.TwoFactorEnabled = true;
            var updateResult = await _userManager.UpdateAsync(user);
            return true;
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ResendVerificationEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User not found"); // User does not exist
            }
            else if (user.EmailConfirmed)
            {
                // Email is already confirmed, no need to resend
                throw new Exception("Email is already confirmed"); // Email is already confirmed
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogInformation("Resending confirmation email to {Email} with token: {Token}", email, token);
            await SendConfirmationEmail(email, token);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User not found");
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Password reset failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            else {
                return true; 
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result ?? throw new NotFoundException("User not found");

            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var resetLink = $"https://localhost:5001/reset-password?email={email}&token={Uri.EscapeDataString(token)}";

            var subject = "Password Reset Request";
            var body = string.Format(EmailTemplates.PasswordResetTemplate, resetLink);
            var isSent = await _emailSender.SendEmail(email, subject, body);
            if (!isSent)
            {
                throw new Exception("Failed to send password reset email, please try again");
            }
            return true;
        }
        
        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User not found");

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Password change failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return true;
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User not found");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("User deletion failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return true;
        }
    }
}