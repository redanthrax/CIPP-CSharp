using MudBlazor;

namespace CIPP.Frontend.Modules.Notifications.Interfaces;

public interface INotificationService {
    void ShowSuccess(string message, string? title = null);
    void ShowError(string message, string? title = null);
    void ShowInfo(string message, string? title = null);
    void ShowWarning(string message, string? title = null);
    Task<bool> ShowConfirmationAsync(string title, string message, string yesText = "Yes", string noText = "No");
    Task ShowErrorDialogAsync(string title, string message, List<string>? errors = null, string? technicalDetails = null);
}
