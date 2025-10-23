using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetTeamsActivityQueryHandler : IRequestHandler<GetTeamsActivityQuery, Task<List<TeamsActivityDto>>> {
    private readonly ITeamsService _teamsService;

    public GetTeamsActivityQueryHandler(ITeamsService teamsService) {
        _teamsService = teamsService;
    }

    public async Task<List<TeamsActivityDto>> Handle(GetTeamsActivityQuery query, CancellationToken cancellationToken) {
        return await _teamsService.GetTeamsActivityAsync(query.TenantId, query.Type, cancellationToken);
    }
}
