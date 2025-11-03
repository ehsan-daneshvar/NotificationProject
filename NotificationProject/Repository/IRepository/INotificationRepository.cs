using NotificationProject.Models;

namespace NotificationProject.Repository.IRepository
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task<Notification?> GetByIdAsync(int id);
        Task<IEnumerable<Notification>> QueryAsync(int? userId = null, int skip = 0, int take = 50);
    }
}
