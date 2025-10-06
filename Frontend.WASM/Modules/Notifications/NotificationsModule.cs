using CIPP.Frontend.WASM.Modules.Notifications.Interfaces;
using CIPP.Frontend.WASM.Modules.Notifications.Services;

namespace CIPP.Frontend.WASM.Modules.Notifications;

public static class NotificationsModule {
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services) {
        services.AddScoped<INotificationService, NotificationService>();
        
        return services;
    }
}