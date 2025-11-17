using CIPP.Frontend.Modules.Notifications.Interfaces;
using CIPP.Shared.DTOs;

namespace CIPP.Frontend.Extensions;

public static class ErrorHandlingExtensions {
    public static void HandleError<T>(
        this Response<T>? response, 
        IErrorHandlingService errorHandler, 
        string context) {
        if (response == null) {
            errorHandler.AddError(context, "No response received from server");
            return;
        }

        if (!response.Success) {
            var technicalDetails = response.Errors?.Any() == true 
                ? string.Join("\n", response.Errors) 
                : null;
            
            errorHandler.AddError(
                context, 
                response.Message ?? "Unknown error occurred", 
                technicalDetails
            );
        }
    }

    public static bool IsSuccess<T>(this Response<T>? response) {
        return response?.Success == true;
    }

    public static T? GetDataOrDefault<T>(this Response<T>? response) {
        return response?.Success == true ? response.Data : default;
    }
}
