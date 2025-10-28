using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class GetStandardTemplatesQueryHandler : IRequestHandler<GetStandardTemplatesQuery, Task<PagedResponse<StandardTemplateDto>>> {
    private readonly IStandardsService _standardsService;

    public GetStandardTemplatesQueryHandler(IStandardsService standardsService) {
        _standardsService = standardsService;
    }

    public async Task<PagedResponse<StandardTemplateDto>> Handle(GetStandardTemplatesQuery request, CancellationToken cancellationToken = default) {
        return await _standardsService.GetTemplatesAsync(request.Type, request.Category, request.PagingParams, cancellationToken);
    }
}
