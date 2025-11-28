using CIPP.Api.Modules.Alerts.Models;

namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IAlertTemplateService {
    Task<AlertTemplate> GenerateAuditLogTemplateAsync(
        Dictionary<string, object> auditData, 
        string tenantId, 
        string? alertComment = null);
    
    string GenerateHtmlEmail(AlertTemplate template, string? alertComment = null);
    string GenerateJsonPayload(AlertTemplate template, string? alertComment = null);
    
    Task<string> GenerateAlertHtmlAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string operation,
        string? alertComment = null);
    
    Task<Dictionary<string, object>> GenerateAlertJsonAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string operation,
        string? alertComment = null);
}
