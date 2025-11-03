using NotificationProject.Models;
using NotificationProject.Service.IService;
using NotificationProject.Utilities;
using static NotificationProject.Utilities.SD;

namespace NotificationProject.Service
{
    public class EmailSender : IMessageSender
    {
        public NotificationChannel Channel => NotificationChannel.Email;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }
        public Task SendAsync(Notification notification)
        {
            _logger.LogInformation("Sending email to user with Id : {UserId}", notification.UserId);
            // 
            // Logic for Sending Email
            //
            return Task.CompletedTask;
        }
    }
}
