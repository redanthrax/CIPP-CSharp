using CIPP.Shared.DTOs.Alerts;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Alerts.Commands;

public record CreateAuditLogAlertCommand(
    CreateAuditLogAlertDto AlertData
) : IRequest<CreateAuditLogAlertCommand, Task<string>>;