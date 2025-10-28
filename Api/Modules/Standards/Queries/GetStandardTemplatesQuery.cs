using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Queries;

public record GetStandardTemplatesQuery(string? Type, string? Category, PagingParameters PagingParams)
    : IRequest<GetStandardTemplatesQuery, Task<PagedResponse<StandardTemplateDto>>>;
