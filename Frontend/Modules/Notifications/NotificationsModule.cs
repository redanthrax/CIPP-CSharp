using CIPP.Frontend.Modules.Notifications.Interfaces;
using CIPP.Frontend.Modules.Notifications.Services;

namespace CIPP.Frontend.Modules.Notifications;

public static class NotificationsModule {
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services) {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IErrorHandlingService, ErrorHandlingService>();
        
        return services;
    }
}
