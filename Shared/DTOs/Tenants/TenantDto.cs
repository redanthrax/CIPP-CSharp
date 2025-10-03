namespace CIPP.Shared.DTOs.Tenants;
public record TenantDto(
    Guid Id,
    string TenantId,
    string DisplayName,
    string DefaultDomainName,
    string Status,
    DateTime CreatedAt,
    string CreatedBy,
    string? Metadata = null
);