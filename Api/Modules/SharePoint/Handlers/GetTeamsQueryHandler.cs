using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, Task<PagedResponse<TeamDto>>> {
    private readonly ITeamsService _teamsService;

    public GetTeamsQueryHandler(ITeamsService teamsService) {
        _teamsService = teamsService;
    }

    public async Task<PagedResponse<TeamDto>> Handle(GetTeamsQuery query, CancellationToken cancellationToken) {
        return await _teamsService.GetTeamsAsync(query.TenantId, query.PagingParams, cancellationToken);
    }
}
