using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Queries;

public record GetSecurityIncidentsQuery(
    string TenantId
) : IRequest<GetSecurityIncidentsQuery, Task<SecurityIncidentsResponseDto>>;
