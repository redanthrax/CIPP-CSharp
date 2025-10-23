using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Commands;

public record CreateSharePointSiteCommand(string TenantId, CreateSharePointSiteDto CreateDto) : IRequest<CreateSharePointSiteCommand, Task<string>>;
