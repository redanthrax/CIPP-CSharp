namespace CIPP.Shared.DTOs.Tenants;

public record AssignedPlanDto(
    string AssignedDateTime,
    string CapabilityStatus,
    string Service,
    string ServicePlanId
);