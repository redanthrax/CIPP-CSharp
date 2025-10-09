namespace CIPP.Shared.DTOs.Tenants;

public record TenantSelectorOptionDto(
    string Value,
    string Label,
    string Type,
    string DefaultDomainName,
    string DisplayName,
    Guid CustomerId,
    string? OffboardingDefaults = null
);