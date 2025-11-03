using NotificationProject.Models;
using static NotificationProject.Utilities.SD;

namespace NotificationProject.Service.IService
{
    public interface INotificationService
    {
        Task<Notification> SendNotificationAsync(int userId,string title,string message,NotificationChannel? notificationChannel);
        Task<IEnumerable<Notification>> GetNotificationsAsync(int? userId, int skip = 0, int take = 50);
        Task<Notification?> GetNotificationById(int Id);
        Task<NotificationChannel> GetDefaultNotificationChannel();
    }
}
