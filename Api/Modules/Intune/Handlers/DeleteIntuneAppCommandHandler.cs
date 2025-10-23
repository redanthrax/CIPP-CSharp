using CIPP.Api.Modules.Intune.Commands;
using CIPP.Api.Modules.Intune.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Handlers;

public class DeleteIntuneAppCommandHandler : IRequestHandler<DeleteIntuneAppCommand, Task> {
    private readonly IIntuneAppService _appService;

    public DeleteIntuneAppCommandHandler(IIntuneAppService appService) {
        _appService = appService;
    }

    public async Task Handle(DeleteIntuneAppCommand command, CancellationToken cancellationToken) {
        await _appService.DeleteAppAsync(command.TenantId, command.AppId, cancellationToken);
    }
}
