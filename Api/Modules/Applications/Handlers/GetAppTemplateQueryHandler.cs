using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Handlers;

public class GetAppTemplateQueryHandler : IRequestHandler<GetAppTemplateQuery, Task<AppTemplateDto?>> {
    private readonly IAppTemplateService _templateService;

    public GetAppTemplateQueryHandler(IAppTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<AppTemplateDto?> Handle(GetAppTemplateQuery query, CancellationToken cancellationToken) {
        return await _templateService.GetTemplateAsync(query.Id, cancellationToken);
    }
}
