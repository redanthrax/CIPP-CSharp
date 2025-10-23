using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Queries;

public record GetAppTemplatesQuery(
    PagingParameters? Paging = null
) : IRequest<GetAppTemplatesQuery, Task<PagedResponse<AppTemplateDto>>>;
