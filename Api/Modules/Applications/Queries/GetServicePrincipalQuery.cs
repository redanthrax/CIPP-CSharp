using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Queries;

public record GetServicePrincipalQuery(
    string TenantId,
    string ServicePrincipalId
) : IRequest<GetServicePrincipalQuery, Task<ServicePrincipalDto?>>;
