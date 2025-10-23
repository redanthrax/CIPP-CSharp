using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Queries;

public record GetConditionalAccessTemplateQuery(Guid Id) 
    : IRequest<GetConditionalAccessTemplateQuery, Task<ConditionalAccessTemplateDto?>>;
