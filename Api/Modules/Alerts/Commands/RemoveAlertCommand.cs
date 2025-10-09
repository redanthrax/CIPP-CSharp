using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Commands;

public record RemoveAlertCommand(
    string Id,
    string EventType
) : IRequest<RemoveAlertCommand, Task<string>>;