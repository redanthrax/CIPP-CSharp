using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Queries;

public record GetComplianceScoreQuery(Guid TenantId)
    : IRequest<GetComplianceScoreQuery, Task<ComplianceScoreDto>>;
