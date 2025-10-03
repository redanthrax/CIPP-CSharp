namespace CIPP.Shared.DTOs.Tenants;
public record CreateTenantDto(
    string TenantId,
    string DisplayName,
    string DefaultDomainName,
    string Status,
    string? Metadata = null
);