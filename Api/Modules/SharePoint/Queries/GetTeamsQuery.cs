using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetTeamsQuery(string TenantId) : IRequest<GetTeamsQuery, Task<List<TeamDto>>>;
