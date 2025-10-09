using CIPP.Shared.DTOs.Alerts;

namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IAlertConfigurationService {
    Task<List<AlertConfigurationDto>> GetAlertConfigurationsAsync();
    Task<string> CreateAuditLogAlertAsync(CreateAuditLogAlertDto alertData);
    Task<string> CreateScriptedAlertAsync(CreateScriptedAlertDto alertData);
    Task<string> RemoveAlertAsync(string id, string eventType);
}
