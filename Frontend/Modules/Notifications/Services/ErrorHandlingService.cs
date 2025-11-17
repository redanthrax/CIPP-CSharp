namespace CIPP.Frontend.Modules.Notifications.Services;

using CIPP.Frontend.Modules.Notifications.Interfaces;

public class ErrorHandlingService : IErrorHandlingService {
    private readonly INotificationService _notificationService;
    private readonly ILogger<ErrorHandlingService> _logger;
    private readonly List<ErrorItem> _errors = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _isShowing = false;

    public ErrorHandlingService(
        INotificationService notificationService,
        ILogger<ErrorHandlingService> logger) {
        _notificationService = notificationService;
        _logger = logger;
    }

    public bool HasErrors => _errors.Any();
    public int ErrorCount => _errors.Count;

    public void AddError(string context, string message, string? technicalDetails = null) {
        _errors.Add(new ErrorItem {
            Context = context,
            Message = message,
            TechnicalDetails = technicalDetails,
            Timestamp = DateTime.UtcNow
        });
        
        _logger.LogError("Error added to queue: [{Context}] {Message}", context, message);
    }

    public void ClearErrors() {
        _errors.Clear();
    }

    public async Task ShowAccumulatedErrorsAsync(string? title = null, string? message = null) {
        if (!HasErrors || _isShowing) {
            return;
        }

        await _semaphore.WaitAsync();
        try {
            if (!HasErrors || _isShowing) {
                return;
            }

            _isShowing = true;

            var errorMessages = _errors
                .Select(e => $"{e.Context}: {e.Message}")
                .ToList();

            var combinedTechnicalDetails = _errors
                .Where(e => !string.IsNullOrEmpty(e.TechnicalDetails))
                .Select(e => $"[{e.Context}] {e.Timestamp:HH:mm:ss}\n{e.TechnicalDetails}")
                .ToList();

            var technicalDetailsString = combinedTechnicalDetails.Any() 
                ? string.Join("\n\n---\n\n", combinedTechnicalDetails) 
                : null;

            await _notificationService.ShowErrorDialogAsync(
                title ?? "Errors Occurred",
                message ?? $"{ErrorCount} error(s) occurred while processing your request.",
                errorMessages,
                technicalDetailsString
            );

            ClearErrors();
        } finally {
            _isShowing = false;
            _semaphore.Release();
        }
    }

    private class ErrorItem {
        public string Context { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? TechnicalDetails { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
