using MudBlazor;

namespace CIPP.Frontend.Modules.Notifications.Interfaces;

public interface INotificationService {
    void ShowSuccess(string message, string? title = null);
    void ShowError(string message, string? title = null);
    void ShowInfo(string message, string? title = null);
    void ShowWarning(string message, string? title = null);
    void ShowApiErrors(string primaryMessage, List<string> errors, string? title = null);
    void ShowDetailedError(string message, List<string> details, string? title = null);
}
