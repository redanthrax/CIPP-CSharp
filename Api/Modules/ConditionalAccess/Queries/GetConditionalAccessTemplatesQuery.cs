using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Queries;

public record GetConditionalAccessTemplatesQuery()
    : IRequest<GetConditionalAccessTemplatesQuery, Task<List<ConditionalAccessTemplateDto>>>;
