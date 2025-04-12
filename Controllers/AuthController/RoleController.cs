using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RLIMS.Common;
using TheBookClub.Services.AuthServices.IAuthServices;

namespace TheBookClub.Controllers.AuthController

{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string rolename)
        {
            var result = await _roleService.AddRoleAsync(rolename);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Data = rolename,
                    Message = "Role added successfully."
                });
            }
            return BadRequest(new ApiResponse
            {
                StatusCode = 400,
                Message = "Failed to add role."
            });
        }

        [HttpDelete("remove-role/{roleId}")]
        public async Task<IActionResult> RemoveRole(string rolename)
        {
            var result = await _roleService.RemoveRoleAsync(rolename);
            if (result)
            {
                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Role removed successfully."
                });
            }
            return BadRequest(new ApiResponse
            {
                StatusCode = 400,
                Message = "Failed to remove role."
            });
        }

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = roles,
                Message = "Roles retrieved successfully."
            });
        }

        [HttpGet("role-exist/{roleName}")]
        public async Task<IActionResult> RoleExists(string roleName)
        {
            var roleExist = await _roleService.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = $"{roleName} Role not found."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = roleExist,
                Message = "Role exists."
            });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-role/{roleId}")]
        public async Task<IActionResult> GetRoleById(Guid roleId)
        {
            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Role not found."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = role,
                Message = "Role retrieved successfully."
            });
        }
    }
}