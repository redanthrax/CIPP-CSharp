using MudBlazor;
using CIPP.Frontend.Modules.Notifications.Interfaces;

namespace CIPP.Frontend.Modules.Notifications.Services;

public class NotificationService : INotificationService {
    private readonly ISnackbar _snackbar;
    private readonly ILogger<NotificationService> _logger;
    private readonly IDialogService _dialogService;

    public NotificationService(ISnackbar snackbar, ILogger<NotificationService> logger, IDialogService dialogService) {
        _snackbar = snackbar;
        _logger = logger;
        _dialogService = dialogService;
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

    public async Task<bool> ShowConfirmationAsync(string title, string message, string yesText = "Yes", string noText = "No") {
        var result = await _dialogService.ShowMessageBox(
            title, 
            message, 
            yesText, 
            cancelText: noText);
        
        return result == true;
    }

    public async Task ShowErrorDialogAsync(string title, string message, List<string>? errors = null, string? technicalDetails = null) {
        var parameters = new DialogParameters<CIPP.Frontend.Components.ErrorDetailsDialog> {
            { x => x.Message, message },
            { x => x.Errors, errors },
            { x => x.TechnicalDetails, technicalDetails }
        };

        var options = new DialogOptions {
            CloseOnEscapeKey = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true
        };

        await _dialogService.ShowAsync<CIPP.Frontend.Components.ErrorDetailsDialog>(title, parameters, options);
        
        _logger.LogError("Error dialog shown: {Title} - {Message}", title, message);
    }
}
