using MudBlazor;
using CIPP.Frontend.WASM.Modules.Notifications.Interfaces;

namespace CIPP.Frontend.WASM.Modules.Notifications.Services;

public class NotificationService : INotificationService {
    private readonly ISnackbar _snackbar;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ISnackbar snackbar, ILogger<NotificationService> logger) {
        _snackbar = snackbar;
        _logger = logger;
    }

    public void ShowSuccess(string message, string? title = null) {
        var displayMessage = string.IsNullOrEmpty(title) ? message : $"{title}: {message}";
        _snackbar.Add(displayMessage, Severity.Success);
        _logger.LogInformation("Success notification: {Message}", displayMessage);
    }

    public void ShowError(string message, string? title = null) {
        var displayMessage = string.IsNullOrEmpty(title) ? message : $"{title}: {message}";
        _snackbar.Add(displayMessage, Severity.Error, configure => {
            configure.RequireInteraction = true;
            configure.ShowCloseIcon = true;
        });
        _logger.LogError("Error notification: {Message}", displayMessage);
    }

    public void ShowInfo(string message, string? title = null) {
        var displayMessage = string.IsNullOrEmpty(title) ? message : $"{title}: {message}";
        _snackbar.Add(displayMessage, Severity.Info);
        _logger.LogInformation("Info notification: {Message}", displayMessage);
    }

    public void ShowWarning(string message, string? title = null) {
        var displayMessage = string.IsNullOrEmpty(title) ? message : $"{title}: {message}";
        _snackbar.Add(displayMessage, Severity.Warning);
        _logger.LogWarning("Warning notification: {Message}", displayMessage);
    }

    public void ShowApiErrors(string primaryMessage, List<string> errors, string? title = null) {
        var displayTitle = title ?? "API Error";
        ShowError(primaryMessage, displayTitle);

        if (errors.Any()) {
            ShowDetailedError("Error Details", errors);
        }
    }

    public void ShowDetailedError(string message, List<string> details, string? title = null) {
        if (!details.Any()) {
            ShowError(message, title);
            return;
        }

        var detailsText = string.Join("\n• ", details);
        var fullMessage = $"{message}\n\nDetails:\n• {detailsText}";
        
        var displayTitle = title ?? "Detailed Error";
        
        _snackbar.Add(fullMessage, Severity.Error, configure => {
            configure.RequireInteraction = true;
            configure.ShowCloseIcon = true;
            configure.VisibleStateDuration = 10000; 
        });
        
        _logger.LogError("Detailed error notification: {Message}. Details: {Details}", 
            message, string.Join("; ", details));
    }
}