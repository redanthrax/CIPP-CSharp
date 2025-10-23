using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetTeamDetailsQuery(string TenantId, string TeamId) : IRequest<GetTeamDetailsQuery, Task<TeamDetailsDto?>>;
