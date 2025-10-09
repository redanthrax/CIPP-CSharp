using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Commands;

public record CreateScriptedAlertCommand(CreateScriptedAlertDto AlertData) : IRequest<CreateScriptedAlertCommand, Task<string>>;
