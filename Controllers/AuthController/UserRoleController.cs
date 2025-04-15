using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheBookClub.Common;
using TheBookClub.Models.Dtos.AuthDtos;
using TheBookClub.Services.AuthServices.IAuthServices;

namespace TheBookClub.Controllers.AuthController
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpPost("add-user-to-role/{userId}/{roleId}")]
        public async Task<IActionResult> AddUserToRole(Guid userId, Guid roleId)
        {
            var result = await _userRoleService.AddUserToRoleAsync(userId, roleId);
            if (result)
            {
                var userRoles = new UserRoleDto
                {
                    UserId = userId,
                    RoleId = roleId
                };
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Data = userRoles,
                    Message = "User added to role successfully."
                });
            }
            return BadRequest(new ApiResponse
            {
                StatusCode = 400,
                Message = "Failed to add user to role."
            });
        }

        [HttpDelete("remove-user-from-role/{userId}/{roleId}")]
        public async Task<IActionResult> RemoveUserFromRole(Guid userId, Guid roleId)
        {
            var result = await _userRoleService.RemoveUserFromRoleAsync(userId, roleId);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "User removed from role successfully."
                });
            }
            return BadRequest(new ApiResponse
            {
                StatusCode = 400,
                Message = "Failed to remove user from role."
            });
        }
        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-user-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var roles = await _userRoleService.GetUserRolesAsync(userId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = roles,
                Message = "User roles retrieved successfully."
            });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("is-user-in-role/{userId}/{roleId}")]
        public async Task<IActionResult> IsUserInRole(Guid userId, Guid roleId)
        {
            var result = await _userRoleService.IsUserInRoleAsync(userId, roleId);

            if (!result)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Data = result,
                    Message = "User is not in the specified role."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = result,
                Message = "User is in the specified role."
            });
        }
    }
}