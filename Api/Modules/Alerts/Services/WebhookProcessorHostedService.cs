using CIPP.Api.Modules.Alerts.Interfaces;

namespace CIPP.Api.Modules.Alerts.Services;

public class WebhookProcessorHostedService : BackgroundService {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WebhookProcessorHostedService> _logger;
    private readonly TimeSpan _processingInterval = TimeSpan.FromSeconds(30);

    public WebhookProcessorHostedService(
        IServiceProvider serviceProvider,
        ILogger<WebhookProcessorHostedService> logger) {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("Webhook Processor Hosted Service started");

        while (!stoppingToken.IsCancellationRequested) {
            try {
                using var scope = _serviceProvider.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<IAuditLogProcessor>();
                
                await processor.ProcessCachedEventsAsync(tenantFilter: null, batchSize: 100);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error processing cached webhook events");
            }

            await Task.Delay(_processingInterval, stoppingToken);
        }

        _logger.LogInformation("Webhook Processor Hosted Service stopped");
    }
}
