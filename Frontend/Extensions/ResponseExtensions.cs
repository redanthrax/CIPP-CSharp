using CIPP.Frontend.Modules.Notifications.Interfaces;
using CIPP.Shared.DTOs;

namespace CIPP.Frontend.Extensions;

public static class ResponseExtensions
{
    public static bool HandleResponse<T>(this Response<T> response, INotificationService notificationService, 
        string? operationName = null, string? successMessage = null)
    {
        if (response.Success)
        {
            if (!string.IsNullOrEmpty(successMessage))
            {
                notificationService.ShowSuccess(successMessage);
            }
            else if (!string.IsNullOrEmpty(response.Message))
            {
                notificationService.ShowSuccess(response.Message);
            }
            return true;
        }
        else
        {
            var title = string.IsNullOrEmpty(operationName) ? "Operation Failed" : $"{operationName} Failed";
            var message = response.Message ?? "An unknown error occurred";
            
            if (response.Errors.Any())
            {
                notificationService.ShowApiErrors(message, response.Errors, title);
            }
            else
            {
                notificationService.ShowError(message, title);
            }
            return false;
        }
    }

    public static bool HandleErrorsOnly<T>(this Response<T> response, INotificationService notificationService,
        string? operationName = null)
    {
        if (response.Success)
        {
            return true;
        }

        var title = string.IsNullOrEmpty(operationName) ? "Error" : $"{operationName} Error";
        var message = response.Message ?? "An unknown error occurred";

        if (response.Errors.Any())
        {
            notificationService.ShowApiErrors(message, response.Errors, title);
        }
        else
        {
            notificationService.ShowError(message, title);
        }
        return false;
    }

    public static async Task<bool> OnSuccess<T>(this Response<T> response, INotificationService notificationService,
        Func<T?, Task> onSuccessAction, string? operationName = null)
    {
        if (response.Success)
        {
            await onSuccessAction(response.Data);
            return true;
        }
        else
        {
            response.HandleErrorsOnly(notificationService, operationName);
            return false;
        }
    }

    public static bool OnSuccess<T>(this Response<T> response, INotificationService notificationService,
        Action<T?> onSuccessAction, string? operationName = null)
    {
        if (response.Success)
        {
            onSuccessAction(response.Data);
            return true;
        }
        else
        {
            response.HandleErrorsOnly(notificationService, operationName);
            return false;
        }
    }

    public static string GetErrorSummary<T>(this Response<T> response)
    {
        if (response.Success) return string.Empty;

        var message = response.Message ?? "An unknown error occurred";
        
        if (response.Errors.Any())
        {
            var keyErrors = response.Errors
                .Where(e => e.StartsWith("Error Code:") || e.StartsWith("Description:"))
                .Take(2)
                .ToList();
                
            if (keyErrors.Any())
            {
                return $"{message}\n{string.Join("\n", keyErrors)}";
            }
        }

        return message;
    }

    public static bool HasErrorCode<T>(this Response<T> response, params string[] errorCodes)
    {
        if (response.Success || !response.Errors.Any()) return false;

        return response.Errors.Any(error => 
            errorCodes.Any(code => error.Contains($"Error Code: {code}") || error.Contains($"MSAL Error Code: {code}")));
    }

    public static bool IsAuthenticationError<T>(this Response<T> response)
    {
        return response.HasErrorCode("unauthorized_client", "invalid_client", "invalid_grant") ||
               response.Errors.Any(e => e.Contains("Authentication Exception") || e.Contains("AuthenticationFailedException"));
    }
}
