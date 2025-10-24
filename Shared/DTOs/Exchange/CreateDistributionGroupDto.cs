namespace CIPP.Shared.DTOs.Exchange;

public class CreateDistributionGroupDto {
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PrimarySmtpAddress { get; set; } = string.Empty;
    public List<string>? ManagedBy { get; set; }
    public List<string>? Members { get; set; }
    public bool RequireSenderAuthenticationEnabled { get; set; } = true;
}
