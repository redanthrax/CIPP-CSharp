using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetTeamDetailsQueryHandler : IRequestHandler<GetTeamDetailsQuery, Task<TeamDetailsDto?>> {
    private readonly ITeamsService _teamsService;

    public GetTeamDetailsQueryHandler(ITeamsService teamsService) {
        _teamsService = teamsService;
    }

    public async Task<TeamDetailsDto?> Handle(GetTeamDetailsQuery query, CancellationToken cancellationToken) {
        return await _teamsService.GetTeamDetailsAsync(query.TenantId, query.TeamId, cancellationToken);
    }
}
