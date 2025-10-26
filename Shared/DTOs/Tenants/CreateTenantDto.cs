namespace CIPP.Shared.DTOs.Tenants;
public record CreateTenantDto(
    Guid TenantId,
    string DisplayName,
    string DefaultDomainName,
    string Status,
    string? Metadata = null
);