using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Commands;

public record UpdateSecurityAlertCommand(
    string TenantId,
    string AlertId,
    UpdateSecurityAlertDto UpdateDto
) : IRequest<UpdateSecurityAlertCommand, Task>;
