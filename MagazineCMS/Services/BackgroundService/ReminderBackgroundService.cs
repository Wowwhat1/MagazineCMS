namespace MagazineCMS.Services.BackgroundService
{
    public class ReminderBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly NotificationSender _notificationSender;

        public ReminderBackgroundService(NotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Run the reminder logic
                _notificationSender.SendContributionReminders();

                // Wait for 24 hours before running again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
