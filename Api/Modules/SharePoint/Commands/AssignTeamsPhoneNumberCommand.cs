using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Commands;

public record AssignTeamsPhoneNumberCommand(string TenantId, AssignTeamsPhoneNumberDto AssignDto) : IRequest<AssignTeamsPhoneNumberCommand, Task<string>>;
