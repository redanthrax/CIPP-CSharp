using CIPP.Api.Modules.Standards.Commands;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class CreateStandardTemplateCommandHandler : IRequestHandler<CreateStandardTemplateCommand, Task<StandardTemplateDto>> {
    private readonly IStandardsService _standardsService;

    public CreateStandardTemplateCommandHandler(IStandardsService standardsService) {
        _standardsService = standardsService;
    }

    public async Task<StandardTemplateDto> Handle(CreateStandardTemplateCommand request, CancellationToken cancellationToken = default) {
        return await _standardsService.CreateTemplateAsync(request.CreateDto, request.CreatedBy, cancellationToken);
    }
}
