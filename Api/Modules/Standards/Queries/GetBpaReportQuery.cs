using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Queries;

public record GetBpaReportQuery(Guid TenantId, string? Category)
    : IRequest<GetBpaReportQuery, Task<BpaReportDto>>;
