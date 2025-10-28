using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Queries;

public record GetStandardTemplateQuery(Guid TemplateId)
    : IRequest<GetStandardTemplateQuery, Task<StandardTemplateDto?>>;
