using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetAppTemplatesQueryHandler : IRequestHandler<GetAppTemplatesQuery, Task<PagedResponse<AppTemplateDto>>> {
    private readonly IAppTemplateService _templateService;

    public GetAppTemplatesQueryHandler(IAppTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<PagedResponse<AppTemplateDto>> Handle(GetAppTemplatesQuery query, CancellationToken cancellationToken) {
        return await _templateService.GetTemplatesAsync(query.Paging, cancellationToken);
    }
}
