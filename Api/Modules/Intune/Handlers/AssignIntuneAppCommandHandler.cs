using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class AssignIntuneAppCommandHandler : IRequestHandler<AssignIntuneAppCommand, Task> {
    private readonly IIntuneAppService _appService;

    public AssignIntuneAppCommandHandler(IIntuneAppService appService) {
        _appService = appService;
    }

    public async Task Handle(AssignIntuneAppCommand command, CancellationToken cancellationToken) {
        await _appService.AssignAppAsync(command.TenantId, command.AppId, command.AssignTo, cancellationToken);
    }
}
