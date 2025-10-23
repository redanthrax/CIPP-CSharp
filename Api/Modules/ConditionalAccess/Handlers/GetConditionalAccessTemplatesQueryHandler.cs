using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class GetConditionalAccessTemplatesQueryHandler : IRequestHandler<GetConditionalAccessTemplatesQuery, Task<List<ConditionalAccessTemplateDto>>> {
    private readonly IConditionalAccessTemplateService _templateService;

    public GetConditionalAccessTemplatesQueryHandler(IConditionalAccessTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<List<ConditionalAccessTemplateDto>> Handle(GetConditionalAccessTemplatesQuery query, CancellationToken cancellationToken) {
        return await _templateService.GetTemplatesAsync(cancellationToken);
    }
}
