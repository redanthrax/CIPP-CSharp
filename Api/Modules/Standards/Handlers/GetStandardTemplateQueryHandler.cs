using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class GetStandardTemplateQueryHandler : IRequestHandler<GetStandardTemplateQuery, Task<StandardTemplateDto?>> {
    private readonly IStandardsService _standardsService;

    public GetStandardTemplateQueryHandler(IStandardsService standardsService) {
        _standardsService = standardsService;
    }

    public async Task<StandardTemplateDto?> Handle(GetStandardTemplateQuery request, CancellationToken cancellationToken = default) {
        return await _standardsService.GetTemplateAsync(request.TemplateId, cancellationToken);
    }
}
