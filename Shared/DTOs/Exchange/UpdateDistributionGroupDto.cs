namespace CIPP.Shared.DTOs.Exchange;

public class UpdateDistributionGroupDto {
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public List<string>? ManagedBy { get; set; }
    public bool? RequireSenderAuthenticationEnabled { get; set; }
}
