using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Queries;

public record GetSecurityAlertQuery(
    string TenantId,
    string AlertId
) : IRequest<GetSecurityAlertQuery, Task<SecurityAlertDto?>>;
