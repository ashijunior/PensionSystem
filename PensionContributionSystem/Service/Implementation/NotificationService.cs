using PensionContributionSystem.Service.Interface;

namespace PensionContributionSystem.Service.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendNotificationAsync(int memberId, string message)
        {
            // Simulate sending an email or SMS
            _logger.LogInformation($"Notification sent to member {memberId}: {message}");
        }
    }
}
