using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Api.Modules.SharePoint.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Task<string>> {
    private readonly ITeamsService _teamsService;

    public CreateTeamCommandHandler(ITeamsService teamsService) {
        _teamsService = teamsService;
    }

    public async Task<string> Handle(CreateTeamCommand command, CancellationToken cancellationToken) {
        return await _teamsService.CreateTeamAsync(command.TenantId, command.CreateDto, cancellationToken);
    }
}
