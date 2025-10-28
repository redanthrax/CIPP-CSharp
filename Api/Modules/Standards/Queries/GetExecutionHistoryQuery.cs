using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Queries;

public record GetExecutionHistoryQuery(Guid? TemplateId, Guid? TenantId, PagingParameters PagingParams)
    : IRequest<GetExecutionHistoryQuery, Task<PagedResponse<StandardExecutionDto>>>;
