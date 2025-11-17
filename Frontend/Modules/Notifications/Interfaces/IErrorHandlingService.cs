namespace CIPP.Frontend.Modules.Notifications.Interfaces;

public interface IErrorHandlingService {
    void AddError(string context, string message, string? technicalDetails = null);
    void ClearErrors();
    Task ShowAccumulatedErrorsAsync(string? title = null, string? message = null);
    bool HasErrors { get; }
    int ErrorCount { get; }
}
