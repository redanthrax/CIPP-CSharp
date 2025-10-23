using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetTeamsActivityQuery(string TenantId, string Type) : IRequest<GetTeamsActivityQuery, Task<List<TeamsActivityDto>>>;
