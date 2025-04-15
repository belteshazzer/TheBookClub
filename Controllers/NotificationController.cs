using TheBookClub.Common;
using Microsoft.AspNetCore.Mvc;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.NotificationService;
using AutoMapper;

namespace TheBookClub.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        [HttpPost("SendNotification/{userId}")]
        public async Task<IActionResult> SendNotification(Guid userId, [FromBody] string message)
        {
            if (string.IsNullOrEmpty(message) || userId == Guid.Empty)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid notification data."
                });
            }
            
            await _notificationService.SendNotificationAsync(userId, message);
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Notification sent successfully."
            });
        }

        [HttpGet("Get-user-notifications/{userId}")]
        public async Task<IActionResult> GetNotifications(Guid userId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(userId);
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = notifications,
                Message = "Notifications retrieved successfully."
            });
        }

        [HttpGet("GetNotification/{id}")]
        public async Task<IActionResult> GetNotification(Guid id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = notification,
                Message = "Notification retrieved successfully."
            });
        }

        [HttpDelete("DeleteNotification/{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            return NoContent();
        }

        [HttpPut("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            await _notificationService.MarkNotificationAsReadAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Notification marked as read."
            });
        }

        [HttpPut("Is-notification-read/{id}")]
        public async Task<IActionResult> IsNotificationRead(Guid id)
        {
            var isRead = await _notificationService.IsNotificationReadAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = isRead,
                Message = "Notification read status retrieved successfully."
            });
        }
    }
}