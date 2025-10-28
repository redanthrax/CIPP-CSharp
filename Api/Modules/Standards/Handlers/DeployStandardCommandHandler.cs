using CIPP.Api.Modules.Standards.Commands;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class DeployStandardCommandHandler : IRequestHandler<DeployStandardCommand, Task<List<StandardResultDto>>> {
    private readonly IStandardsService _standardsService;

    public DeployStandardCommandHandler(IStandardsService standardsService) {
        _standardsService = standardsService;
    }

    public async Task<List<StandardResultDto>> Handle(DeployStandardCommand request, CancellationToken cancellationToken = default) {
        return await _standardsService.DeployStandardAsync(request.DeployDto, request.ExecutedBy, cancellationToken);
    }
}
