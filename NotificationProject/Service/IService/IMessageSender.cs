using NotificationProject.Models;
using static NotificationProject.Utilities.SD;

namespace NotificationProject.Service.IService
{
    public interface IMessageSender
    {
        public NotificationChannel Channel { get; }
        public Task SendAsync(Notification notification);
    }
}
