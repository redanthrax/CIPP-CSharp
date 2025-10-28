using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class GetExecutionHistoryQueryHandler : IRequestHandler<GetExecutionHistoryQuery, Task<PagedResponse<StandardExecutionDto>>> {
    private readonly IStandardsService _standardsService;

    public GetExecutionHistoryQueryHandler(IStandardsService standardsService) {
        _standardsService = standardsService;
    }

    public async Task<PagedResponse<StandardExecutionDto>> Handle(GetExecutionHistoryQuery request, CancellationToken cancellationToken = default) {
        return await _standardsService.GetExecutionHistoryAsync(request.TemplateId, request.TenantId, request.PagingParams, cancellationToken);
    }
}
