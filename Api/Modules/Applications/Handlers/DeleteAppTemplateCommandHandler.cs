using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class DeleteAppTemplateCommandHandler : IRequestHandler<DeleteAppTemplateCommand, Task> {
    private readonly IAppTemplateService _templateService;

    public DeleteAppTemplateCommandHandler(IAppTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task Handle(DeleteAppTemplateCommand command, CancellationToken cancellationToken) {
        await _templateService.DeleteTemplateAsync(command.Id, cancellationToken);
    }
}
