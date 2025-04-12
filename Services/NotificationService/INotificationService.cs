using TheBookClub.Models.Entities;

namespace TheBookClub.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendNotificationAsync(Guid userId, string message);
        Task SendGroupNotificationAsync(IEnumerable<Guid> groupId, string message);
        Task DeleteNotificationAsync(Guid id);
        Task<IEnumerable<Notification>> GetNotificationsAsync(Guid userId);
        Task<Notification> GetNotificationByIdAsync(Guid id);
        Task MarkNotificationAsReadAsync(Guid id);
        Task SoftDeleteNotificationAsync(Guid id);
        Task<bool> IsNotificationReadAsync(Guid id);
    }
}