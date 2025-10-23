using CIPP.Api.Modules.Alerts.Interfaces;
using Hangfire;

namespace CIPP.Api.Modules.Alerts.Services;

public class AlertJobService : IAlertJobService {
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ILogger<AlertJobService> _logger;

    public AlertJobService(
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJobManager,
        ILogger<AlertJobService> logger) {
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
        _logger = logger;
    }

    public Task<string> ScheduleRecurringAlertAsync(string jobId, Guid alertConfigurationId, string cronExpression) {
        try {
            _logger.LogInformation("Scheduling recurring alert job {JobId} for configuration {ConfigurationId} with cron {CronExpression}", 
                jobId, alertConfigurationId, cronExpression);
            
            _recurringJobManager.AddOrUpdate<AlertExecutionService>(
                jobId,
                service => service.ExecuteAlertAsync(alertConfigurationId),
                cronExpression,
                new RecurringJobOptions {
                    TimeZone = TimeZoneInfo.Utc
                });

            return Task.FromResult(jobId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to schedule recurring alert job {JobId}", jobId);
            throw new InvalidOperationException($"Failed to schedule recurring alert job: {ex.Message}");
        }
    }

    public Task<string> ScheduleAlertAsync(Guid alertConfigurationId, int delayInSeconds = 0) {
        try {
            _logger.LogInformation("Scheduling one-time alert job for configuration {ConfigurationId} with delay {DelaySeconds}s", 
                alertConfigurationId, delayInSeconds);
            
            string jobId;
            if (delayInSeconds > 0) {
                jobId = _backgroundJobClient.Schedule<AlertExecutionService>(
                    service => service.ExecuteAlertAsync(alertConfigurationId),
                    TimeSpan.FromSeconds(delayInSeconds));
            } else {
                jobId = _backgroundJobClient.Enqueue<AlertExecutionService>(
                    service => service.ExecuteAlertAsync(alertConfigurationId));
            }

            return Task.FromResult(jobId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to schedule alert job for configuration {ConfigurationId}", alertConfigurationId);
            throw new InvalidOperationException($"Failed to schedule alert job: {ex.Message}");
        }
    }

    public Task RemoveJobAsync(string jobId) {
        try {
            _logger.LogInformation("Removing job {JobId}", jobId);
            _recurringJobManager.RemoveIfExists(jobId);
            _backgroundJobClient.Delete(jobId);
            return Task.CompletedTask;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove job {JobId}", jobId);
            throw new InvalidOperationException($"Failed to remove job: {ex.Message}");
        }
    }

    public async Task ExecuteAlertAsync(Guid alertConfigurationId) {
        try {
            _logger.LogInformation("Executing immediate alert for configuration {ConfigurationId}", alertConfigurationId);
            _backgroundJobClient.Enqueue<AlertExecutionService>(
                service => service.ExecuteAlertAsync(alertConfigurationId));
            await Task.CompletedTask;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to execute alert for configuration {ConfigurationId}", alertConfigurationId);
            throw new InvalidOperationException($"Failed to execute alert: {ex.Message}");
        }
    }
}