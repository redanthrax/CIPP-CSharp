using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Alerts;

namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IAlertConfigurationService {
    Task<PagedResponse<AlertConfigurationDto>> GetAlertConfigurationsAsync(PagingParameters? paging = null);
    Task<string> CreateAuditLogAlertAsync(CreateAuditLogAlertDto alertData);
    Task<string> CreateScriptedAlertAsync(CreateScriptedAlertDto alertData);
    Task<string> RemoveAlertAsync(string id, string eventType);
}
