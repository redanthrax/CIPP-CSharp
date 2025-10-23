using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Commands;

public record UpdateSecureScoreControlCommand(
    string TenantId,
    string ControlName,
    UpdateSecureScoreControlDto UpdateDto
) : IRequest<UpdateSecureScoreControlCommand, Task>;
