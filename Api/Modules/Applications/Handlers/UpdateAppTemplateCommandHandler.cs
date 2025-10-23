using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class UpdateAppTemplateCommandHandler : IRequestHandler<UpdateAppTemplateCommand, Task<AppTemplateDto>> {
    private readonly IAppTemplateService _templateService;

    public UpdateAppTemplateCommandHandler(IAppTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<AppTemplateDto> Handle(UpdateAppTemplateCommand command, CancellationToken cancellationToken) {
        return await _templateService.UpdateTemplateAsync(command.Id, command.UpdateDto, cancellationToken);
    }
}
