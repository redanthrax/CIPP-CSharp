using CIPP.Api.Modules.Tenants.Models;
using DispatchR.Abstractions.Send;
namespace CIPP.Api.Modules.Tenants.Commands;
public record CreateTenantCommand(
    string TenantId,
    string DisplayName,
    string DefaultDomainName,
    string Status,
    string CreatedBy,
    string? Metadata = null
) : IRequest<CreateTenantCommand, Task<Tenant>>;
