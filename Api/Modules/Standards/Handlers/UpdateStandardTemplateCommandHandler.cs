using CIPP.Api.Modules.Standards.Commands;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class UpdateStandardTemplateCommandHandler : IRequestHandler<UpdateStandardTemplateCommand, Task<StandardTemplateDto?>> {
    private readonly IStandardsService _standardsService;

    public UpdateStandardTemplateCommandHandler(IStandardsService standardsService) {
        _standardsService = standardsService;
    }

    public async Task<StandardTemplateDto?> Handle(UpdateStandardTemplateCommand request, CancellationToken cancellationToken = default) {
        return await _standardsService.UpdateTemplateAsync(request.TemplateId, request.UpdateDto, request.ModifiedBy, cancellationToken);
    }
}
