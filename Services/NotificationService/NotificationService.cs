using Microsoft.AspNetCore.SignalR;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IGenericRepository<Notification> _repository;

        public NotificationService(IHubContext<NotificationHub> hubContext, IGenericRepository<Notification> repository)
        {
            _hubContext = hubContext;
            _repository = repository;
        }

        public async Task SendNotificationAsync(Guid userId, string message)
        {
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
        }

        public async Task SendGroupNotificationAsync(IEnumerable<Guid> userIds, string message)
        {
            foreach (var userId in userIds)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
                await _repository.AddAsync(new Notification
                {
                    UserId = userId,
                    Message = message,
                    IsRead = false,
                });
            }
        }

        public async Task DeleteNotificationAsync(Guid id)
        {
            var notification = await _repository.GetByIdAsync(id);
            if (notification != null)
            {
                await _repository.DeleteAsync(id);
            }
        }

        public async Task SoftDeleteNotificationAsync(Guid id)
        {
            var notification = await _repository.GetByIdAsync(id);
            if (notification != null)
            {
                notification.IsDeleted = true;
                await _repository.UpdateAsync(notification);
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(Guid userId)
        {
            return await _repository.GetByConditionAsync(n => n.UserId == userId);
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task MarkNotificationAsReadAsync(Guid id)
        {
            var notification = await _repository.GetByIdAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _repository.UpdateAsync(notification);
            }
        }

        public async Task<bool> IsNotificationReadAsync(Guid id)
        {
            var notification = await _repository.GetByIdAsync(id);
            return notification != null && notification.IsRead;
        }
    }
}