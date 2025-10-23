using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Api.Modules.SharePoint.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class RemoveTeamsPhoneNumberCommandHandler : IRequestHandler<RemoveTeamsPhoneNumberCommand, Task<string>> {
    private readonly ITeamsService _teamsService;

    public RemoveTeamsPhoneNumberCommandHandler(ITeamsService teamsService) {
        _teamsService = teamsService;
    }

    public async Task<string> Handle(RemoveTeamsPhoneNumberCommand command, CancellationToken cancellationToken) {
        return await _teamsService.RemovePhoneNumberAsync(command.TenantId, command.RemoveDto, cancellationToken);
    }
}
