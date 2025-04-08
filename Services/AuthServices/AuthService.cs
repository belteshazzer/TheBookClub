using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TheBookClub.Common;
using TheBookClub.Common.EmailSender;
using TheBookClub.Common.Exceptions;
using TheBookClub.Models.Dtos.AuthDtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                user.UserName = user.Email; // Default to email if username is not provided
            }

            // Attempt to create the user
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
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
            var confirmationLink = $"https://localhost:5001/confirm-email?email={email}&token={Uri.EscapeDataString(token)}";
            var subject = "Password Reset Request";
            var body = string.Format(EmailTemplates.ConfirmEmailTemplate, confirmationLink);
            var emailSent = await _emailSender.SendEmail(email, subject, body);
            if (!emailSent)
            {
                throw new Exception("Failed to send confirmation email, please try again");
            }
        }

        public async Task VerifyEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new Exception("Email confirmation failed");
            }
        }

        public async Task<LoginResponse> LoginUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User not found"); // User does not exist
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {               
                var token = GenerateJwtToken(user);
                var loginResponse = new LoginResponse
                {
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(1) 
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

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "User")
            };

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
    }
}