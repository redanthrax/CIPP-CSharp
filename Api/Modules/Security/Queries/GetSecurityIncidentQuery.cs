using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Queries;

public record GetSecurityIncidentQuery(
    string TenantId,
    string IncidentId
) : IRequest<GetSecurityIncidentQuery, Task<SecurityIncidentDto?>>;
