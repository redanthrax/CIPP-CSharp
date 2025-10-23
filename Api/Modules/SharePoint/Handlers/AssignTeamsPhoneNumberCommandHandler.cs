using CIPP.Api.Modules.SharePoint.Commands;
using CIPP.Api.Modules.SharePoint.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Handlers;

public class AssignTeamsPhoneNumberCommandHandler : IRequestHandler<AssignTeamsPhoneNumberCommand, Task<string>> {
    private readonly ITeamsService _teamsService;

    public AssignTeamsPhoneNumberCommandHandler(ITeamsService teamsService) {
        _teamsService = teamsService;
    }

    public async Task<string> Handle(AssignTeamsPhoneNumberCommand command, CancellationToken cancellationToken) {
        return await _teamsService.AssignPhoneNumberAsync(command.TenantId, command.AssignDto, cancellationToken);
    }
}
