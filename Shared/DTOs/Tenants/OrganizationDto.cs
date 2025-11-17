namespace CIPP.Shared.DTOs.Tenants;

public class OrganizationDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public List<VerifiedDomainDto> VerifiedDomains { get; set; } = new();
    public bool? OnPremisesSyncEnabled { get; set; }
    public List<AssignedPlanDto> AssignedPlans { get; set; } = new();
}
