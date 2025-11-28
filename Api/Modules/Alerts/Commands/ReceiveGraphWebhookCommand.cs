using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Commands;

public record ReceiveGraphWebhookCommand(string NotificationPayload) : IRequest<ReceiveGraphWebhookCommand, Task>;
