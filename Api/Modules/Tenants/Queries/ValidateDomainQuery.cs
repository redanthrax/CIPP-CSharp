using CIPP.Shared.DTOs.Tenants;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Queries;

public record ValidateDomainQuery(string TenantName)
    : IRequest<ValidateDomainQuery, Task<ValidateDomainResponseDto>>;
