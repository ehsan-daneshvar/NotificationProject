using NotificationProject.Models;
using NotificationProject.Service.IService;
using static NotificationProject.Utilities.SD;

namespace NotificationProject.Service
{
    public class SmsSender : IMessageSender
    {
        public NotificationChannel Channel => NotificationChannel.Sms;

        private readonly ILogger<SmsSender> _logger;
        public SmsSender(ILogger<SmsSender> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(Notification notification)
        {
            _logger.LogInformation("Sending Sms to user with Id :{UserId}", notification.UserId);
            // 
            // Logic for Sending Sms
            //
            return Task.CompletedTask;
        }
    }
}
