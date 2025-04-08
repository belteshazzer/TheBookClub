using Microsoft.AspNetCore.Mvc;
using TheBookClub.Common;
using TheBookClub.Services.AuthServices;
using TheBookClub.Models.Entities;
using TheBookClub.Models.Dtos.AuthDtos;
using AutoMapper;
using RLIMS.Common;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace TheBookClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authServices;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authServices, IMapper mapper, ILogger<AuthController> logger)
        {
            _authServices = authServices ?? throw new ArgumentNullException(nameof(authServices));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
    
            var user = _mapper.Map<User>(request);
            _logger.LogInformation("Registering user: {Email}", user.Email);
            
            var results = await _authServices.RegisterUserAsync(user, request.PasswordHash);

            if (results == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Data = null,
                    Message = "Registration failed. Please try again."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200 ,
                Data = results,
                Message = "Registration successful" 
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Data = null,
                    Message = "Email and password are required."
                });
            }

            var result = await _authServices.LoginUserAsync(request.Email, request.Password);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = result,
                Message = "Login successful"
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
           
            await _authServices.LogoutUserAsync();
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = null,
                Message = "Logout successful"
            });
        }

        [HttpPost("resend-verification-email")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationRequest request)
        {

            var result = await _authServices.ResendVerificationEmailAsync(request.Email);
            return Ok(new ApiResponse
            {
                StatusCode = result ? 200 : 400,
                Data = null,
                Message = result ? "Verification email resent successfully" : "Failed to resend verification email. Please try again."
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
          
            var result = await _authServices.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
            return Ok(new ApiResponse
            {
                StatusCode = result ? 200 : 400,
                Data = null,
                Message = result ? "Password reset successfully" : "Failed to reset password. Please try again."
            });
        }

        [HttpPost("send-password-reset-email")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody] PasswordResetRequest request)
        {
           
            var result = await _authServices.SendPasswordResetEmailAsync(request.Email);
            return Ok(new ApiResponse
            {
                StatusCode = result? 200 : 400,
                Data = null,
                Message = result? "Password reset email sent successfully. Please check your inbox." : "Failed to send password reset email. Please try again."
            });
         
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
           var result = await _authServices.ChangePasswordAsync(request.Email, request.OldPassword, request.NewPassword);
            return Ok(new ApiResponse
            {
                StatusCode = result ? 200 : 400,
                Data = null,
                Message = result ? "Password changed successfully" : "Failed to change password. Please try again."
            });
        }
    }
}