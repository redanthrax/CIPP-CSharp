namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IAlertJobService {
    Task<string> ScheduleRecurringAlertAsync(string jobId, Guid alertConfigurationId, string cronExpression);
    Task<string> ScheduleAlertAsync(Guid alertConfigurationId, int delayInSeconds = 0);
    Task RemoveJobAsync(string jobId);
    Task ExecuteAlertAsync(Guid alertConfigurationId);
}