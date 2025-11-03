using NotificationProject.Models;
using NotificationProject.Repository.IRepository;
using NotificationProject.Service.IService;
using NotificationProject.Utilities;
using static NotificationProject.Utilities.SD;

namespace NotificationProject.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEnumerable<IMessageSender> _messageSenders;
        private readonly IConfiguration _configuration;

        public NotificationService(ILogger<NotificationService> logger, INotificationRepository notificationRepository, IEnumerable<IMessageSender> messageSenders, IConfiguration configuration)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
            _messageSenders = messageSenders;
            _configuration = configuration;
        }

        public async Task<NotificationChannel> GetDefaultNotificationChannel()
        {
            var defaultChannel = _configuration.GetValue<string>("NotificationConfig:DefaultChannel");
            if (Enum.TryParse<NotificationChannel>(defaultChannel, true, out var d)) return d;

            //If Default From appsettings is broken then return hard-coded chaneel 
            return NotificationChannel.Email;
        }

        public async Task<Notification?> GetNotificationById(int Id)
        {
            return await _notificationRepository.GetByIdAsync(Id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int? userId, int skip = 0, int take = 50)
        {
            return await _notificationRepository.QueryAsync(userId, skip, take);
        }

        public async Task<Notification> SendNotificationAsync(int userId, string title, string message, SD.NotificationChannel? notificationChannel)
        {
            NotificationChannel channel = notificationChannel ?? await GetDefaultNotificationChannel();
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Channel = channel,
                Status = NotificationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _logger.LogInformation("Creating notification for user with Id: {UserId} via {Channel}", userId, channel);
            await _notificationRepository.AddAsync(notification);

            var sender = _messageSenders.FirstOrDefault(u => u.Channel == channel);
            if (sender == null)
            {
                notification.Status = NotificationStatus.Failed;
                notification.Error = $"sender not found";
                await _notificationRepository.UpdateAsync(notification);
                _logger.LogError("No sender found for channel {Channel}", channel);
                return notification;
            }
            try
            {
                await sender.SendAsync(notification);
                notification.Status = NotificationStatus.Sent;
                notification.SentAt = DateTime.Now;
                await _notificationRepository.UpdateAsync(notification);
                _logger.LogInformation("Notification with Id : {NotificationId} sent successfully", notification.Id);
            }
            catch (Exception ex)
            {
                notification.Status = NotificationStatus.Failed;
                notification.Error = ex.Message;
                await _notificationRepository.UpdateAsync(notification);
                _logger.LogError(ex, "Error sending notification for Id : {NotificationId}", notification.Id);
            }

            return notification;
        }
    }
}
