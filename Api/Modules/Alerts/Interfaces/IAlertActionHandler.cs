namespace CIPP.Api.Modules.Alerts.Interfaces;

public interface IAlertActionHandler {
    string ActionType { get; }
    Task ExecuteAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string? alertComment = null,
        Dictionary<string, string>? additionalParams = null,
        CancellationToken cancellationToken = default);
}
