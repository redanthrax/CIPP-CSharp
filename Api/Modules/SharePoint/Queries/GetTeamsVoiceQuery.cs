using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetTeamsVoiceQuery(Guid TenantId, PagingParameters PagingParams)
    : IRequest<GetTeamsVoiceQuery, Task<PagedResponse<TeamsVoiceDto>>>;
