using CIPP.Api.Modules.Applications.Commands;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class CreateAppTemplateCommandHandler : IRequestHandler<CreateAppTemplateCommand, Task<AppTemplateDto>> {
    private readonly IAppTemplateService _templateService;

    public CreateAppTemplateCommandHandler(IAppTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<AppTemplateDto> Handle(CreateAppTemplateCommand command, CancellationToken cancellationToken) {
        return await _templateService.CreateTemplateAsync(command.CreateDto, cancellationToken);
    }
}
