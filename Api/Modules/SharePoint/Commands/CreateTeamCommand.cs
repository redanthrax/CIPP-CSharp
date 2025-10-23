using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Commands;

public record CreateTeamCommand(string TenantId, CreateTeamDto CreateDto) : IRequest<CreateTeamCommand, Task<string>>;
