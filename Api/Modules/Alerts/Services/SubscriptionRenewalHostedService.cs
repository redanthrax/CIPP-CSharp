using CIPP.Api.Modules.Alerts.Interfaces;

namespace CIPP.Api.Modules.Alerts.Services;

public class SubscriptionRenewalHostedService : BackgroundService {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SubscriptionRenewalHostedService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6);

    public SubscriptionRenewalHostedService(
        IServiceProvider serviceProvider,
        ILogger<SubscriptionRenewalHostedService> logger) {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("Subscription Renewal Hosted Service started");

        while (!stoppingToken.IsCancellationRequested) {
            try {
                using var scope = _serviceProvider.CreateScope();
                var webhookService = scope.ServiceProvider.GetRequiredService<IGraphWebhookService>();
                
                await webhookService.RenewExpiringSubscriptionsAsync();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error renewing expiring subscriptions");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Subscription Renewal Hosted Service stopped");
    }
}
