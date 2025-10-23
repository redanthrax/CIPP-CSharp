using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class GetTeamsVoiceQueryHandler : IRequestHandler<GetTeamsVoiceQuery, Task<List<TeamsVoiceDto>>> {
    private readonly ITeamsService _teamsService;

    public GetTeamsVoiceQueryHandler(ITeamsService teamsService) {
        _teamsService = teamsService;
    }

    public async Task<List<TeamsVoiceDto>> Handle(GetTeamsVoiceQuery query, CancellationToken cancellationToken) {
        return await _teamsService.GetTeamsVoiceAsync(query.TenantId, cancellationToken);
    }
}
